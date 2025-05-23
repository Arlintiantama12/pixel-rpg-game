# Pixel RPG Game 🎮

Unity 2D RPG game dengan mobile controls dan 4 character classes.

## 📱 Features Yang Sudah Ada
- ✅ **4 Character Classes**: Warrior, Mage, Archer, Rogue
- ✅ **Mobile Controls**: Virtual joystick + touch buttons  
- ✅ **Combat System**: Attack, Dash, Class abilities
- ✅ **Level/XP System**: Experience dan stat growth
- ✅ **Android Build**: APK ready untuk testing

## 🎯 Character Classes
| Class | HP | MP | ATK | DEF | Speed | Weapon |
|-------|----|----|-----|-----|-------|---------|
| **Warrior** | 120 | 30 | 25 | 15 | 4.0 | Sword |
| **Mage** | 80 | 100 | 30 | 5 | 3.0 | Staff |
| **Archer** | 90 | 60 | 22 | 8 | 5.0 | Bow |
| **Rogue** | 85 | 50 | 20 | 6 | 6.0 | Dagger |

## 🕹️ Controls
### Desktop:
- **Movement**: WASD / Arrow Keys
- **Attack**: Left Mouse / Z
- **Dash**: Space
- **Abilities**: 1, 2 keys

### Mobile:
- **Movement**: Virtual Joystick (left)
- **Attack**: ATK Button (right)
- **Dash**: DASH Button (right)
- **Abilities**: 1, 2 Buttons (right)

## 🛠️ Technology Stack
- **Engine**: Unity 6.1 (6000.1.4f1)
- **Platform**: Android (API 24+)
- **Architecture**: ARM64 + ARMv7
- **Scripting**: C# with IL2CPP

## 📂 Project Structure
```
Assets/
├── Scripts/
│   ├── PlayerController.cs      # Main player logic
│   ├── MobileControls.cs        # Mobile input system
│   ├── MobileControlsFixed.cs   # Enhanced mobile controls
│   ├── BuildOptimizer.cs        # Performance optimization
│   ├── TestGameUI.cs           # Test UI system
│   └── MobileDebugTester.cs    # Debug tools
├── Scenes/
│   └── SampleScene.unity       # Main game scene
├── Sprites/                    # Game graphics
├── Audio/                      # Sound effects
└── Prefabs/                    # Reusable objects
```

## 🚀 Build Instructions
1. Open project in Unity 6.1+
2. Switch platform to Android
3. File → Build Settings
4. Add SampleScene to build
5. Build APK to `Builds/` folder

## 📱 Mobile Testing
- **Target**: Android 7.0+ (API 24)
- **Orientation**: Landscape
- **Resolution**: 16:9 optimized
- **Performance**: 60 FPS target

## 🎮 Gameplay Features
### Combat System:
- **Basic Attack**: Different per class
- **Dash**: Quick movement ability
- **Class Abilities**: 2 unique skills per class

### RPG Elements:
- **Experience Points**: 100 XP per level
- **Stat Growth**: HP, MP, ATK increases per level
- **Health/Mana**: Regeneration system

## 🔧 Next Development Plans
1. **Enemy System** 🐉 - 21 monster types (small→boss)
2. **UI/HUD System** 📱 - Health bars, inventory, minimap
3. **Visual Polish** 🎨 - 64x64 pixel art, animations
4. **Level Design** 🗺️ - Dungeons, environments

## 🐛 Known Issues
- Mobile controls need fine-tuning
- Button responsiveness on some devices
- UI scaling on different screen sizes

## 💻 Development Environment
- **Unity Version**: 6.1 (6000.1.4f1)
- **IDE**: Visual Studio Code / Visual Studio
- **Version Control**: Git
- **Platform**: Windows 11

## 📄 License
This project is for educational/portfolio purposes.

## 🤝 Contributing
1. Fork the repository
2. Create feature branch
3. Make changes
4. Test on Android device
5. Submit pull request

## 📞 Contact
Feel free to open issues for bugs or feature requests!

---
**Status**: ✅ Core systems complete | 🔄 Enemy system in development