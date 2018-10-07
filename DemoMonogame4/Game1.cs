using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DemoMonogame4
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		private Model model;

		private Matrix BlenderRotateFix => Matrix.CreateRotationX(MathHelper.ToRadians(-90)) * Matrix.CreateRotationY(MathHelper.ToRadians(-90));

		private Vector3 CubeRotation;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			this.IsMouseVisible = true;
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			model = Content.Load<Model>("textured_cube");

			CubeRotation = new Vector3(0, 0, 0);
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			CubeRotation.X += MathHelper.ToRadians(1);
			CubeRotation.X = CubeRotation.X % MathHelper.ToRadians(360);

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			foreach (var mesh in model.Meshes)
			{
				foreach (var effect1 in mesh.Effects)
				{
					var effect = (BasicEffect)effect1;

					// default lighting
					effect.EnableDefaultLighting();
					effect.PreferPerPixelLighting = true;


					// rotation, translation, scaling
					effect.World = BlenderRotateFix;
					effect.World *= Matrix.CreateRotationY(MathHelper.ToRadians(45));
					effect.World *= Matrix.CreateRotationX(CubeRotation.X);
					effect.World *= Matrix.CreateRotationY(CubeRotation.Y);
					effect.World *= Matrix.CreateRotationZ(CubeRotation.Z);


					var cameraPosition = new Vector3(0, 0, 10);
					var cameraLookAtVector = Vector3.Zero;
					var cameraUpVector = Vector3.UnitY;

					effect.View = Matrix.CreateLookAt(cameraPosition, cameraLookAtVector, cameraUpVector);
					float aspectRatio = graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
					float fieldOfView = MathHelper.PiOver4;
					float nearClipPlane = 1;
					float farClipPlane = 200;
					effect.Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
				}

				mesh.Draw();
			}

			base.Draw(gameTime);
		}
	}
}
