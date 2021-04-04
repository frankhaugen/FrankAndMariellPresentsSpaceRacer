using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Code.IO.Json
{
    [Serializable]
    public class JsonContainer<T>
    {
        public List<T> Entities;
    }
    
    public class JsonContext<TEntity> where TEntity : JsonEntity, new()
    {

        private JsonContainer<TEntity> _jsonContainer;
        private List<TEntity> _collection;
        private readonly List<TEntity> _tempCollection;
        private readonly JsonConfiguration _options;
        private readonly string _filePath;
        private bool _unsavedChanges;

        public JsonContext(JsonConfiguration options = null)
        {
            _options = options ?? new JsonConfiguration();
            
            _collection = new List<TEntity>();
            _tempCollection = new List<TEntity>();

            if (string.IsNullOrWhiteSpace(_options!.Folder)) _options.Folder = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            _filePath = Path.Combine(_options.Folder, typeof(TEntity).Name + ".json");
            Setup();
        }

        private void Setup()
        {
            if (!Directory.Exists(_options.Folder))
            {
                Directory.CreateDirectory(_options.Folder!);
            }

            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, _jsonContainer.ToJson());
            }
            // _collection = File.ReadAllText(_filePath).FromJson<List<TEntity>>();
            _jsonContainer = File.ReadAllText(_filePath).FromJson<JsonContainer<TEntity>>();
        }

        public IEnumerable<TEntity> GetCollection()
        {
            if (!_collection.Any())
                _collection = File.ReadAllText(_filePath).FromJson<List<TEntity>>();

            return _collection;
        }

        private List<TEntity> GetEntities()
        {
            var json = File.ReadAllText(_filePath);

            Debug.Log(json);
            var jsonContainer = JsonUtility.FromJson<JsonContainer<TEntity>>(json);
            
            return jsonContainer.Entities;
        }

        public IQueryable<TEntity> GetQueryable()
        {
            if (!_collection.Any())
                _collection = File.ReadAllText(_filePath).FromJson<List<TEntity>>();

            return _collection.AsQueryable();
        }

        public TEntity GetById(Guid id)
        {
            var entities = GetEntities().FirstOrDefault(x => x.Id == id)!;

            return entities;
        }

        public void Delete(Guid id)
        {
            var entity = _collection.FirstOrDefault(x => x.Id == id)!;
            _collection.Remove(entity);
            _unsavedChanges = true;
        }

        public void Add(TEntity item)
        {
            Debug.Log(item.Id);

            // _tempCollection.Add(item);
            // _unsavedChanges = true;

            var entities = GetEntities();
            Debug.Log(entities.Count());
            entities.Add(item);
            Debug.Log(entities.Count());

            _jsonContainer.Entities = entities;
            
            var json = JsonUtility.ToJson(_jsonContainer, true);
            Debug.Log(json);
            File.WriteAllText(_filePath, json);
        }

        public void Add(List<TEntity> items)
        {
            _tempCollection.AddRange(items);
            _unsavedChanges = true;
        }

        public bool SaveChanges()
        {
            if (!_unsavedChanges) return true;

            GetCollection();
            _collection.AddRange(_tempCollection);
            File.WriteAllText(_filePath, _collection.ToJson());
            _tempCollection.Clear();

            GetCollection();

            _unsavedChanges = false;
            return true;
        }

        public void DiscardChanges()
        {
            _tempCollection.Clear();
            _unsavedChanges = false;
        }

        public string GetFilepath() => _filePath;
    }
}
