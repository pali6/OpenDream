﻿using Robust.Shared.GameObjects;
using Robust.Shared.Serialization;
using System;
using Robust.Shared.GameStates;
using OpenDreamShared.Dream;

namespace OpenDreamShared.Rendering {
    [NetworkedComponent]
    public class SharedDMISpriteComponent : Component {
        public override string Name => "DMISprite";

        [Serializable, NetSerializable]
        protected class DMISpriteComponentState : ComponentState {
            public readonly uint? AppearanceId;
            public readonly ScreenLocation ScreenLocation;

            public DMISpriteComponentState(uint? appearanceId, ScreenLocation screenLocation) {
                AppearanceId = appearanceId;
                ScreenLocation = screenLocation;
            }
        }
    }
}
