using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Code.Extensions;
using Code.Store;

namespace Code.IO.Json
{
    public class JsonContext<T> where T : JsonEntity
    {
        private List<T> _collection;
        private readonly List<T> _tempCollection;
        private bool _unsavedChanges;

        private readonly FileInfo _file;

        public JsonContext()
        {
            _collection = new List<T>();
            _tempCollection = new List<T>();
            _file = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "Data", $"{typeof(T).Name}s.json"));
            
            if (_file.Directory != null && !_file.Directory.Exists) _file.Directory.Create();
            if (!_file.Exists) _file.WriteObject(new JsonWrapper<T>(){Entities = new List<T>()});
            ReadFile();
        }
        private void SaveFile() => _file.WriteObject(new JsonWrapper<T> {Entities = _collection});
        private void ReadFile() => _collection = _file.ReadObject<JsonWrapper<T>>().Entities;

        public void Add(List<T> items) => items.ForEach(Add);
        public IQueryable<T> GetQueryable() => _collection.AsQueryable();
        public T GetById(ulong id) => _collection.FirstOrDefault(x => x.Id == id)!;

        public void Delete(ulong id)
        {
            var entity = _collection.FirstOrDefault(x => x.Id == id)!;
            _collection.Remove(entity);
            _unsavedChanges = true;
        }

        public void Add(T item)
        {
            item.Id = Convert.ToUInt64(_collection.LongCount() + 1);
            item.SessionId = SessionStore.SessionId;
            _tempCollection.Add(item);
            _unsavedChanges = true;
        }
      
        public void SaveChanges()
        {
            if (!_unsavedChanges) return;

            ReadFile();
            _collection.AddRange(_tempCollection);

            SaveFile();
            
            _tempCollection.Clear();
            ReadFile();

            _unsavedChanges = false;
        }

        public void DiscardChanges()
        {
            _tempCollection.Clear();
            _unsavedChanges = false;
        }
    }
}
