﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using oldgoldmine_game.Engine;


namespace oldgoldmine_game.Gameplay
{
    public class Collectible : GameObject3D
    {
        private const bool debugDrawHitbox = true;

        private BoundingBox hitbox;

        public override Vector3 Position
        {
            get { return base.position; }
            set
            {
                Vector3 deltaMovement = value - base.Position;
                base.Position = value;
                hitbox.Min += deltaMovement;
                hitbox.Max += deltaMovement;
            }
        }

        public override Vector3 Scale
        {
            get { return base.scale; }
            set
            {
                Vector3 hitboxOffset = (hitbox.Max + hitbox.Min) / 2 - base.Position;
                hitboxOffset = (hitboxOffset / base.Scale) * value;
                Vector3 boxSize = ((hitbox.Max - hitbox.Min) / base.Scale) * value;
                base.Scale = value;
                hitbox.Min = Position + hitboxOffset - boxSize / 2;
                hitbox.Max = Position + hitboxOffset + boxSize / 2;
            }
        }



        public Collectible()
            : base()
        {
            this.hitbox = new BoundingBox(-0.5f * Vector3.One, 0.5f * Vector3.One);
        }

        public Collectible(Model model)
            : base(model)
        {
            CreateBoundingBoxFromModel(model);
        }

        public Collectible(Model model, Vector3 position, Vector3 scale, Quaternion rotation)
            : base(model, position, scale, rotation)
        {
            base.Scale = Vector3.One;   // to ensure consistency with the hitbox size

            CreateBoundingBoxFromModel(model);

            this.Position = position;
            this.Scale = scale;
        }

        public Collectible(Model model, Vector3 position, Vector3 scale, Quaternion rotation, Vector3 hitboxSize)
            : base(model, position, scale, rotation)
        {
            this.hitbox = new BoundingBox(position - hitboxSize/2, position + hitboxSize/2);
        }

        public Collectible(GameObject3D collectibleObj, Vector3 hitboxSize)
            : base(collectibleObj)
        {
            this.hitbox = new BoundingBox(collectibleObj.Position - hitboxSize / 2,
                collectibleObj.Position + hitboxSize / 2);
        }

        public Collectible(Model model, Vector3 position, Vector3 scale, Quaternion rotation, BoundingBox hitbox)
            : base(model, position, scale, rotation)
        {
            this.hitbox = hitbox;
        }

        public Collectible(GameObject3D collectibleObj, BoundingBox hitbox)
            : base(collectibleObj)
        {
            this.hitbox = hitbox;
        }


        // Create the bounding box by taking the bounds of all the model meshes
        // and merging them together into a single bounding box
        void CreateBoundingBoxFromModel(in Model model)
        {
            this.hitbox = BoundingBox.CreateFromSphere(model.Meshes[0].BoundingSphere);
            for (int meshIndex = 1; meshIndex < model.Meshes.Count; meshIndex++)
            {
                BoundingBox meshBox = BoundingBox.CreateFromSphere(model.Meshes[meshIndex].BoundingSphere);
                this.hitbox = BoundingBox.CreateMerged(this.hitbox, meshBox);
            }
        }


        /// <summary>
        /// Change the position of this collectible in 3D coordinate space.
        /// </summary>
        /// <param name="movement">A Vector3 representing the amount of movement to apply on each axis.</param>
        public override void MovePosition(Vector3 movement)
        {
            this.Position += movement;
        }

        /// <summary>
        /// Change the scale (size) of the Collectible object, both the model and its hitbox.
        /// </summary>
        /// <param name="scale">A value representing the uniform scaling factor for the entire object.</param>
        public override void ScaleSize(float scale)
        {
            this.Scale = new Vector3(scale, scale, scale);
        }

        /// <summary>
        /// Change the scale (size) of the Collectible object, both the model and its hitbox.
        /// </summary>
        /// <param name="scale">A Vector3 representing the scaling factors for each axis.</param>
        public override void ScaleSize(Vector3 scale)
        {
            this.Scale = scale;
        }


        public void Update()
        {
            if (!IsActive)
                return;

            if (CheckPlayerCollision(OldGoldMineGame.player))
            {
                this.IsActive = false;
                OldGoldMineGame.UpdateScore(100);
            }
        }


        bool CheckPlayerCollision(Player player)
        {
            if (this.hitbox.Intersects(player.hitbox))
                return true;
            return false;
        }


        public override void Draw(in GameCamera camera)
        {
            if (!IsActive)
                return;

            base.Draw(camera);

            if (debugDrawHitbox)
            {
                OldGoldMineGame.basicEffect.Projection = OldGoldMineGame.player.Camera.Projection;
                OldGoldMineGame.basicEffect.View = OldGoldMineGame.player.Camera.View;

                Vector3[] vertices = hitbox.GetCorners();

                // Pairs of points define the lines (segments) which are the border of the box to draw
                VertexPositionColor[] lineVertices = new VertexPositionColor[24]
                {
                    new VertexPositionColor(vertices[0], Color.Green),
                    new VertexPositionColor(vertices[1], Color.Green),
                    new VertexPositionColor(vertices[0], Color.Green),
                    new VertexPositionColor(vertices[4], Color.Green),
                    new VertexPositionColor(vertices[0], Color.Green),
                    new VertexPositionColor(vertices[3], Color.Green),
                    new VertexPositionColor(vertices[1], Color.Green),
                    new VertexPositionColor(vertices[2], Color.Green),
                    new VertexPositionColor(vertices[1], Color.Green),
                    new VertexPositionColor(vertices[5], Color.Green),
                    new VertexPositionColor(vertices[2], Color.Green),
                    new VertexPositionColor(vertices[3], Color.Green),
                    new VertexPositionColor(vertices[2], Color.Green),
                    new VertexPositionColor(vertices[6], Color.Green),
                    new VertexPositionColor(vertices[3], Color.Green),
                    new VertexPositionColor(vertices[7], Color.Green),
                    new VertexPositionColor(vertices[7], Color.Green),
                    new VertexPositionColor(vertices[4], Color.Green),
                    new VertexPositionColor(vertices[4], Color.Green),
                    new VertexPositionColor(vertices[5], Color.Green),
                    new VertexPositionColor(vertices[5], Color.Green),
                    new VertexPositionColor(vertices[6], Color.Green),
                    new VertexPositionColor(vertices[6], Color.Green),
                    new VertexPositionColor(vertices[7], Color.Green)
                };

                OldGoldMineGame.basicEffect.CurrentTechnique.Passes[0].Apply();
                OldGoldMineGame.graphics.GraphicsDevice.
                    DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, lineVertices, 0, 12);
            }
        }


        public override object Clone()
        {
            return new Collectible(this.model3d, this.position, this.scale, this.rotation, this.hitbox);
        }

    }
}