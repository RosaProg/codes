using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;

using System.Threading;

using TestXNA_blabla;

using TestXNA.EstrategiasDibujo;

using Logging;

using TexturedQuad;

namespace TestXNA
{
    /// <summary>
    /// Game
    /// </summary>
    /// 

    public enum TiposMapa
    {
        GananciaDiaria = 0,
        GananciaHora = 1,
        TiempoUsoContadores = 2,
        TiempoUsoEventos = 3
    }


    public class MapaTermico : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        frmControles ctrl;

        EstrategiaDibujo e;
        public void SetTipoMapa(EstrategiaDibujo e)
        {
            this.e = e;
            UpdateMapa();
        }

        bool update_mapa = false;
        public void UpdateMapa()
        {
            update_mapa = true;
        }

        public MapaTermico()
        {
            Logs.Logfile = "MapaTermico";
            Logs.InitializeLogfiles();
            Logs.InitializeErrfile();

            ShowGearIcon = true;
            ctrl = new frmControles(this);
            ctrl.Show();

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = int.Parse(ConfigurationSettings.AppSettings["g.bufferHeight"]);
            graphics.PreferredBackBufferWidth = int.Parse(ConfigurationSettings.AppSettings["g.bufferWidth"]);
            icon_rectangle = new Rectangle(graphics.PreferredBackBufferWidth - 75,
                25, 50, 50);
            gear_rectangle = new Rectangle(graphics.PreferredBackBufferWidth - 75,
                100, 50, 50);

            //SCALE = float.Parse(ConfigurationSettings.AppSettings["g.scale"]);
            SCALE = 0.1f;
            modelScale = Matrix.CreateScale(float.Parse(ConfigurationSettings.AppSettings["g.world.scale"]));

            name_pos = new Vector2(50, graphics.PreferredBackBufferHeight - 75);

            graphics.PreferMultiSampling = true;
            //graphics.IsFullScreen = true;
            this.IsMouseVisible = true;
            
            
            //graphics.ToggleFullScreen();
            System.Windows.Forms.Form frm = (System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(Window.Handle);
            frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            frm.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        }

        class Camera
        {
            public Vector3 Position;
            public Vector3 Target;
            public Vector3 UpVector;

            public void Update()
            {
                View = Matrix.CreateLookAt(Position, Target, UpVector);
            }

            public Matrix View { get; private set; }
        }

        class MovingCamera : Camera
        {
            public Vector3 InitialPosition;
            public Vector3 InitialTarget;

            public void MoveToOrigin()
            {
                Vector3 d = Position - InitialPosition;
                if (d.Length() < 0.01f)
                    Position = InitialPosition;
                else
                    Position = InitialPosition + 0.98f * d;

                d = Target - InitialTarget;
                if (d.Length() < 0.01f)
                    Target = InitialTarget;
                else
                    Target = InitialTarget + 0.98f * d;

                Update();
            }
        }

        MovingCamera camera;


        class SceneObject
        {
            public string Name;
            public Model Model;

            public Vector3 Location;
            public Vector3 Angle = new Vector3(0, 0, 0);

            public Vector3 LinearSpeed = new Vector3(0, 0, 0);
            public Vector3 RotationalSpeed = new Vector3(0, 0, 0);

            public Vector3 Light;
            public float Alpha;

            public Vector3 BlinkingLight = new Vector3(0.0f,1.0f,0.0f);

            public BoundingBox BoundingBox;
        }

        object lock_scene_objects = new object();
        List<SceneObject> scene_objects = new List<SceneObject>();

        float SCALE = 3f; // 1f/10f;

        public void Close()
        {
            thr_update.Abort();
            this.Exit();
        }

        SceneObject selected = null;
        bool online = true;
        void LoadEGMs()
        {
            try
            {
                List<SceneObject> tmp_objects = new List<SceneObject>();

                e.StartData();
                tmp_objects.Clear();

                DbProviderFactory f = DbProviderFactories.GetFactory("System.Data.SqlClient");

                DbConnection c = f.CreateConnection();
                c.ConnectionString = ConfigurationSettings.AppSettings["db.connectionString"];
                c.Open();

                DbCommand cmd = f.CreateCommand();
                cmd.Connection = c;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = e.GetQuery();
                cmd.CommandText = cmd.CommandText.Replace("<%=db%>", ConfigurationSettings.AppSettings["db.name"]);
                cmd.CommandText = cmd.CommandText.Replace("<%=olddb%>", ConfigurationSettings.AppSettings["db.oldname"]);
                cmd.CommandText = cmd.CommandText.Replace("<%=padron%>", ConfigurationSettings.AppSettings["db.padron"]);
                Debug.WriteLine(cmd.CommandText);

                int mid_x = 0, mid_y = 0, count = 0, max = 0;

                DataSet ds = new DataSet();
                DbDataAdapter da = f.CreateDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                DbDataReader dr = ds.CreateDataReader();

                //DbDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                //foreach ( DataRow dr in ds.Tables[0].Rows )
                {
                    e.PrepareData(dr);
                    int x = (int)dr[1];
                    int y = (int)dr[2];

                    mid_x += x;
                    mid_y += y;
                    count++;

                    if (x > max) max = x;
                    if (y > max) max = y;
                }

                if (count != 0)
                {
                    mid_x /= count;
                    mid_y /= count;
                }

                mid_x = int.Parse(ConfigurationSettings.AppSettings["g.midx"]);
                mid_y = int.Parse(ConfigurationSettings.AppSettings["g.midy"]);

                dr.Close();

                if (count != 0)
                {
                    //dr = cmd.ExecuteReader();
                    dr = ds.CreateDataReader();

                    while (dr.Read())
                    {
                        int x = (int)dr[1] - mid_x;
                        int y = (int)dr[2] - mid_y;

                        if ((string)dr[0] == "0010394")
                        {
                            int k = 9;
                        }

                        SceneObject o = new SceneObject
                        {
                            Name = (string) dr[0],
                            Location = new Vector3(SCALE * x, 0, SCALE * y),
                            Angle = new Vector3(0.0f, (float)(int)dr[3], 0.0f),
                            RotationalSpeed = new Vector3(0, 0, 0),
                            Light = e.GetLight(dr),
                            BlinkingLight = e.BlinkingLight(dr),
                            Alpha = e.GetAlpha(dr),
                            Model = Content.Load<Model>("MV_Converted")
                        };

                        o.BoundingBox = CreateBoundingBox(o.Model, o.Location);
                        tmp_objects.Add(o);

                        if (selected == null)
                            selected = o;
                    }

                    c.Close();
                    online = true;
                    lock ( lock_scene_objects )
                        scene_objects = tmp_objects;

                    e.EndData();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + ex.StackTrace);
                Logs.LogError("UNHANDLED EXCEPTION: " + ex.Message + ex.StackTrace);
                online = false;
            }
        }

        BoundingBox CreateBoundingBox(Model model, Vector3 location)
        {
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), Matrix.CreateTranslation(location) * modelScale);

                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }

            BoundingBox retval = new BoundingBox(min, max);
            //Debug.WriteLine("BB: " + retval.ToString());

            return retval;
        }
         
        Thread thr_update;
        Quad quad_piso;

        Quad quad_test;
        protected override void Initialize()
        {
            int distance = int.Parse(ConfigurationSettings.AppSettings["g.camera.initialDistance"]);
            camera = new MovingCamera 
            { 
                Position = new Vector3(0, distance, distance), 
                Target = new Vector3(0, 0, 0), 
                UpVector = -Vector3.UnitZ,
                InitialPosition = new Vector3(0,distance,distance),
                InitialTarget = new Vector3(0,0,0)
            };

            camera.Update();

            egm = Content.Load<Texture2D>("index");
            LoadEGMs();

            thr_update = new Thread(new ThreadStart(UpdateThread));
            thr_update.Start();

            quad_piso = new Quad(Vector3.Zero, Vector3.Backward, Vector3.Up, 1, 1);
            quad_test = new Quad(Vector3.Zero, Vector3.Backward, Vector3.Up, 1, 1);
            base.Initialize();
        }

        Texture2D egm;

        SpriteFont font;
        Texture2D gear;
        Texture2D piso;
        BasicEffect quad_piso_effect;

        BasicEffect quadEffect;
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");

            online_icon = Content.Load<Texture2D>("onAlpha");
            offline_icon = Content.Load<Texture2D>("offAlpha");
            gear = Content.Load<Texture2D>("icon_gear");
            piso = Content.Load<Texture2D>("enemigo");

            quad_piso_effect = new BasicEffect(graphics.GraphicsDevice);
            quad_piso_effect.EnableDefaultLighting();
            quad_piso_effect.World = Matrix.Identity;
            quad_piso_effect.View = camera.View;
            quad_piso_effect.Projection = projection;
            quad_piso_effect.TextureEnabled = true;
            quad_piso_effect.Texture = piso;

            quadEffect = new BasicEffect(graphics.GraphicsDevice);
            quadEffect.EnableDefaultLighting();

            quadEffect.World = Matrix.Identity;
            quadEffect.View = Matrix.CreateLookAt(new Vector3(0, 80, 80), Vector3.Zero,
                -Vector3.UnitZ); ;
            quadEffect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 200f);             
            quadEffect.TextureEnabled = true;
            quadEffect.Texture = Content.Load<Texture2D>("Glass");

            vertexDeclaration = new VertexDeclaration(new VertexElement[]
                {
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                    new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
                }
            );
        }

        VertexDeclaration vertexDeclaration;


        protected override void UnloadContent()
        {
            // Unload of any non ContentManager content here
        }

        double update_seconds = 10.0;
        DateTime lastUpdate = DateTime.Now;

        void UpdateThread()
        {
            while (true)
            {
                while (!(update_mapa || DateTime.Now.Subtract(lastUpdate).TotalSeconds > update_seconds))
                    Thread.Sleep(500);

                update_mapa = false;
                lastUpdate = DateTime.Now;
                if (e.Update())
                    LoadEGMs();
            }
        }

        bool IsInside(MouseState mstate, Rectangle r)
        {
            return mstate.X >= r.X && mstate.X <= (r.X + r.Width) &&
                   mstate.Y >= r.Y && mstate.Y <= (r.Y + r.Height);
        }

        bool show_only_blinking = false;

        bool blink = false;
        DateTime last_blink = DateTime.Now;
        protected override void Update(GameTime gameTime)
        {
            if (DateTime.Now.Subtract(last_blink).TotalMilliseconds > 1000)
            {
                last_blink = DateTime.Now;
                blink = !blink;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Space))
                disparo.Disparar(150, 100);

            if (state.IsKeyDown(Keys.R))
                camera.Position -= new Vector3(0, 1, 1);
            else if (state.IsKeyDown(Keys.F))
                camera.Position += new Vector3(0, 1, 1);
            else if (state.IsKeyDown(Keys.A))
            {
                camera.Position += new Vector3(-1, 0, 0);
                camera.Target += new Vector3(-1, 0, 0);
            }
            else if (state.IsKeyDown(Keys.D))
            {
                camera.Position += new Vector3(1, 0, 0);
                camera.Target += new Vector3(1, 0, 0);
            }
            else if (state.IsKeyDown(Keys.W))
            {
                camera.Position += new Vector3(0, 0, -1);
                camera.Target += new Vector3(0, 0, -1);
            }
            else if (state.IsKeyDown(Keys.S))
            {
                camera.Position += new Vector3(0, 0, 1);
                camera.Target += new Vector3(0, 0, 1);
            }
            else if (state.IsKeyDown(Keys.Q))
                Exit();
            else if (state.IsKeyDown(Keys.C))
            {
                camera.MoveToOrigin();
            }
            else if (state.IsKeyDown(Keys.B))
                show_only_blinking = true;
            else if (state.IsKeyDown(Keys.N))
                show_only_blinking = false;
            else if (state.IsKeyDown(Keys.D0))
                    e.Tipo = null;
            else if (state.IsKeyDown(Keys.D1))
                    e.Tipo = 1;
            else if (state.IsKeyDown(Keys.D2))
                    e.Tipo = 2;
            else if (state.IsKeyDown(Keys.D3))
                    e.Tipo = 3;
            else if (state.IsKeyDown(Keys.D4))
                    e.Tipo = 4;
            else if (state.IsKeyDown(Keys.D5))
                    e.Tipo = 5;

            MouseState mstate = Mouse.GetState();
            if ( mstate.LeftButton == ButtonState.Pressed && 
                 IsInside(mstate, gear_rectangle) )
            {
                ctrl.WindowState = System.Windows.Forms.FormWindowState.Normal;
            }
            else if (mstate.LeftButton == ButtonState.Pressed)
            {
                Vector3 nearsource = new Vector3((float) mstate.X, (float) mstate.Y, 0f);
                Vector3 farsource = new Vector3((float) mstate.X, (float) mstate.Y, 1f);

                Matrix world = Matrix.CreateTranslation(0, 0, 0);

                Vector3 nearPoint = GraphicsDevice.Viewport.Unproject(nearsource,
                    projection, camera.View, world);

                Vector3 farPoint = GraphicsDevice.Viewport.Unproject(farsource,
                    projection, camera.View, world);

                Vector3 direction = farPoint - nearPoint;
                direction.Normalize();
                Ray pickRay = new Ray(nearPoint, direction);

                float min_distance = float.MaxValue;
                foreach (SceneObject obj in scene_objects)
                {
                    float? distance = pickRay.Intersects(obj.BoundingBox);
                    if ( distance != null )
                        if (distance < min_distance)
                        {
                            min_distance = (float) distance;
                            uid = obj.Name;
                            last_uid = DateTime.Now;
                        }
                }
            }

            camera.Update();
            lock ( lock_scene_objects )
                foreach (SceneObject o in scene_objects)
                {
                    o.Location += o.LinearSpeed;
                    o.Angle += o.RotationalSpeed;
                }

            base.Update(gameTime);
        }

        string uid = "";
        DateTime last_uid = DateTime.Now;

        Matrix world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
//        Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), Vector3.UnitY);
        Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 2000f);

        const int EGMS = 20;
        Vector3[] egm_positions = new Vector3[EGMS];

        Vector3 redLight = new Vector3(1.0f, 0, 0);
        Vector3 greenLight = new Vector3(0, 1.0f, 0);

        Matrix modelScale;

        int count = 0;
        void DrawModel(SceneObject obj, Model model, Matrix world, Matrix view, Matrix projection)
        {
            if (show_only_blinking && obj.BlinkingLight == obj.Light)
                return;

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.EnableDefaultLighting();
                    
                    if ((count & 1) == 0)
                        effect.AmbientLightColor = new Vector3(0, 1.0f, 0);
                    else
                        effect.AmbientLightColor = new Vector3(0, 0, 1.0f);

                    effect.AmbientLightColor = blink ? obj.BlinkingLight : obj.Light;
                    effect.LightingEnabled = true;
                    effect.World = world * modelScale;
                    effect.View = view;
                    effect.Projection = projection;
                    effect.Alpha = obj.Alpha;
                }

                mesh.Draw();
            }
            count++;
        }

        float DistanceToCamera(Camera camera, Vector3 pos)
        {
            float dx = camera.Position.X - pos.X;
            float dy = camera.Position.Y - pos.Y;
            float dz = camera.Position.Z - pos.Z;

            return (float) Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

//        void DrawModels(List<RenderedObject> 

        void DrawScene(Camera camera, List<SceneObject> objects)
        {
            objects.Sort(new Comparison<SceneObject>(CompareSceneObjects));
            foreach (SceneObject o in objects)
            {
                Matrix world = Matrix.Identity;
                if (o.Angle.X != 0) world *= Matrix.CreateRotationX(o.Angle.X);
                if (o.Angle.Y != 0) world *= Matrix.CreateRotationY(o.Angle.Y);
                if (o.Angle.Z != 0) world *= Matrix.CreateRotationZ(o.Angle.Z);
                world *= Matrix.CreateTranslation(o.Location);

                DrawModel(o, o.Model, world, camera.View, projection);
            }
        }

        int CompareSceneObjects(SceneObject a, SceneObject b)
        {
            return DistanceToCamera(camera, b.Location).CompareTo(DistanceToCamera(camera, a.Location));
        } 

        public bool ShowGearIcon { get; set; }

        Disparo disparo = new Disparo();
        Texture2D online_icon, offline_icon;
        Rectangle icon_rectangle = new Rectangle(1000, 100, 100, 100);
        Rectangle gear_rectangle = new Rectangle(1000, 300, 100, 100);

        Vector2 text_pos = new Vector2(50, 50);
        Vector2 name_pos = new Vector2(50, 800);
        const int line_height = 40;
        protected override void Draw(GameTime gameTime)
        {
            if (online)
                GraphicsDevice.Clear(Color.CornflowerBlue);
            else
                GraphicsDevice.Clear(Color.Red);

            /*
            quad_piso_effect.World = world;
            quad_piso_effect.View = camera.View;
            foreach (EffectPass pass in quad_piso_effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>
                    (
                        PrimitiveType.TriangleList,
                        quad_piso.Vertices, 0, 4,
                        quad_piso.Indexes, 0, 2);
            }
            */

            lock (scene_lock)
                DrawScene(camera, scene_objects);

            spriteBatch.Begin();
            spriteBatch.DrawString(font, e.GetTitle(), text_pos, Color.White);

            if ( DateTime.Now.Subtract(last_uid).TotalSeconds < 10 )
                spriteBatch.DrawString(font, uid, name_pos, Color.White);

            Vector2 pos = new Vector2(text_pos.X, text_pos.Y + line_height);
            string[] legends = e.GetLegends();
            for (int i = 0; i < legends.Length; i++)
                if (legends[i].Length != 0)
                    spriteBatch.DrawString(font, legends[i], pos, Color.White);

            spriteBatch.Draw(online ? online_icon : offline_icon, icon_rectangle, Color.White);
            if (ShowGearIcon)
                spriteBatch.Draw(gear, gear_rectangle, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        object scene_lock = new object();
    }
}
