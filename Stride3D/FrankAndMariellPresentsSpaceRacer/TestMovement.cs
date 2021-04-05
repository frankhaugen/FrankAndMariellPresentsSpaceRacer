using Stride.Core.Diagnostics;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Extensions;
using Stride.Graphics;
using Stride.Graphics.GeometricPrimitives;
using Stride.Input;
using Stride.Physics;
using Stride.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrankAndMariellPresentsSpaceRacer
{
    public class TestMovement : SyncScript
    {
        public List<Thruster> Thrusters { get; set; }
        //public List<Thruster> Thrusters = new List<Thruster>();
        //public List<string> Thrusters = new List<string>();
        //public List<Thruster> Thrusters = new List<Thruster>();
        //public Thruster Thruster { get; set; } = new Thruster();
        private RigidbodyComponent _rigidbody;

        public override void Start()
        {
            _rigidbody = Entity.Get<RigidbodyComponent>();

            //Thrusters.Add(Thruster);
            //Thrusters.AddRange(Entity.GetAll<Thruster>());

            // Initialization of the script.
            Log.ActivateLog(LogMessageType.Debug);

            Log.Debug("Logging activated!");
        }

        public override void Update()
        {
            // Do stuff every new frame


            try
            {
                if (Input.HasDownKeys)
                {
                    //Thrusters.Where(x => Input.DownKeys.Contains(x.Key)).ToList().ForEach(x => x.Execute(_rigidbody));

                    //if (Input.DownKeys.Any(x => x == Keys.W))
                    //{

                    //    _rigidbody.ApplyForce(Vector3.UnitZ * 5);
                    //}
                }

                if (Input.HasPressedKeys)
                {
                    if (Input.PressedKeys.Any(x => x == Keys.Space))
                    {
                        Entity.Scene.Entities.Add(new Entity(Vector3.UnitY));
                        Log.Debug("Adding!");
                    }
                }
            }
            catch (Exception e)
            {
                //Log.Error(e.Message, e);
                Log.Error(e.Message, e);
            }
        }

        private void BuggyCreateTriangle()
        {

            // Create an entity and add it to the scene.
            var entity = new Entity();
            SceneSystem.SceneInstance.RootScene.Entities.Add(entity);

            // Create a model and assign it to the model component.
            var model = new Model();
            entity.GetOrCreate<ModelComponent>().Model = model;

            // Add one or more meshes using geometric primitives (eg spheres or cubes).
            var meshDraw = GeometricPrimitive.Sphere.New(GraphicsDevice).ToMeshDraw();

            var mesh = new Mesh { Draw = meshDraw };
            model.Meshes.Add(mesh);
            // Create a mesh using your own vertex and index buffers.

            mesh = new Mesh { Draw = new MeshDraw { /* Vertex buffer and index buffer setup */ } };
            model.Meshes.Add(mesh);


            var vertices = new VertexPositionTexture[3];
            vertices[0].Position = new Vector3(0f, 0f, 1f);
            vertices[1].Position = new Vector3(0f, 1f, 0f);
            vertices[2].Position = new Vector3(0f, 1f, 1f);
            var vertexBuffer = Stride.Graphics.Buffer.Vertex.New(GraphicsDevice, vertices,
                GraphicsResourceUsage.Dynamic);
            int[] indices = { 0, 2, 1 };
            var indexBuffer = Stride.Graphics.Buffer.Index.New(GraphicsDevice, indices);

            var customMesh = new Stride.Rendering.Mesh
            {
                Draw = new Stride.Rendering.MeshDraw
                {
                    /* Vertex buffer and index buffer setup */
                    PrimitiveType = Stride.Graphics.PrimitiveType.TriangleList,
                    DrawCount = indices.Length,
                    IndexBuffer = new IndexBufferBinding(indexBuffer, true, indices.Length),
                    VertexBuffers = new[] { new VertexBufferBinding(vertexBuffer,
                                    VertexPositionTexture.Layout, vertexBuffer.ElementCount) },
                }
            };
            // add the mesh to the model
            model.Meshes.Add(customMesh);
        }
    }
}
