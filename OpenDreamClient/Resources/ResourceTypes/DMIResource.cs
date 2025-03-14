﻿using System;
using System.Collections.Generic;
using System.IO;
using OpenDreamClient.Input;
using OpenDreamShared.Dream;
using OpenDreamShared.Resources;
using Robust.Client.Graphics;
using Robust.Shared.IoC;
using Robust.Shared.Maths;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace OpenDreamClient.Resources.ResourceTypes {
    public class DMIResource : DreamResource {
        private readonly byte[] _pngHeader = { 0x89, 0x50, 0x4E, 0x47, 0xD, 0xA, 0x1A, 0xA };

        public Texture Texture;
        public Vector2i IconSize;
        public DMIParser.ParsedDMIDescription Description;

        private Dictionary<string, State> _states;

        public DMIResource(string resourcePath, byte[] data) : base(resourcePath, data)
        {
            if (!IsValidPNG()) throw new Exception("Attempted to create a DMI using an invalid PNG");

            Stream dmiStream = new MemoryStream(data);
            DMIParser.ParsedDMIDescription description = DMIParser.ParseDMI(dmiStream);

            dmiStream.Seek(0, SeekOrigin.Begin);

            Image<Rgba32> image = Image.Load<Rgba32>(dmiStream);
            Texture = IoCManager.Resolve<IClyde>().LoadTextureFromImage(image);
            IconSize = new Vector2i(description.Width, description.Height);
            Description = description;

            IClickMapManager clickMapManager = IoCManager.Resolve<IClickMapManager>();

            _states = new Dictionary<string, State>();
            foreach (DMIParser.ParsedDMIState parsedState in description.States.Values) {
                State state = new State(Texture, parsedState, description.Width, description.Height);

                _states.Add(parsedState.Name, state);
                clickMapManager.CreateClickMap(state, image);
            }
        }

        public State? GetState(string stateName) {
            if (stateName == null || !_states.ContainsKey(stateName)) return null;

            return _states[stateName];
        }

        private bool IsValidPNG() {
            if (Data.Length < _pngHeader.Length) return false;

            for (int i=0; i<_pngHeader.Length; i++) {
                if (Data[i] != _pngHeader[i]) return false;
            }

            return true;
        }

        public struct State {
            public Dictionary<AtomDirection, AtlasTexture[]> Frames;

            public State(Texture texture, DMIParser.ParsedDMIState parsedState, int width, int height) {
                Frames = new Dictionary<AtomDirection, AtlasTexture[]>();

                foreach (KeyValuePair<AtomDirection, DMIParser.ParsedDMIFrame[]> pair in parsedState.Directions) {
                    AtomDirection dir = pair.Key;
                    DMIParser.ParsedDMIFrame[] parsedFrames = pair.Value;
                    AtlasTexture[] frames = new AtlasTexture[parsedFrames.Length];

                    for (int i = 0; i < parsedFrames.Length; i++) {
                        DMIParser.ParsedDMIFrame parsedFrame = parsedFrames[i];

                        frames[i] = new AtlasTexture(texture, new UIBox2(parsedFrame.X, parsedFrame.Y, parsedFrame.X + width, parsedFrame.Y + height));
                    }

                    Frames.Add(dir, frames);
                }
            }

            public AtlasTexture[] GetFrames(AtomDirection direction) {
                if (!Frames.TryGetValue(direction, out AtlasTexture[] frames))
                    frames = Frames[AtomDirection.South];

                return frames;
            }
        }

    }
}
