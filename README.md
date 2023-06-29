# mayhem
Clone of this game https://play.google.com/store/apps/details?id=com.rocketjump.majormayhem2

**Gameplay description**

Player automatically run from left to right until they encounter a cover and stop behind it. Shooting is available by tapping on the screen while running or in cover.
Enemies behave in different ways: idle, hiding behind their cover, jumping. Standart bullets don't harm the player but red ones do. Player can headshot enemies.
One enemies stage can consists of a few waves. Enemies can appear between the stages while player is running.

**Code architecture**

Player and Enemy scripts are inherited from Unit because they share some similarities like losing health
Fire of Player and enemies is controlled by separate scripts PlayerFireController and EnemyFireController and they are inherited from FireController
All scripts are mainly interact with each other through events
In some cases scripts like GameManager and Player can control all other scripts inside them
All managers and Player are initialized in GameManager so that it controls the order of initialization
Player gameobject has some separate scripts including Player.cs that controls different types of behaviour like running, hiding in cover, firing etc.

**Assets used**

Movement here is mainly implemented with DoTweenPro https://assetstore.unity.com/packages/tools/visual-scripting/dotween-pro-32416 
except when enemies are dying I turn their Rigidbodies isKinematic to false and apply force
All animations are from Mixamo and player character as well
Enemies models are from this asset https://assetstore.unity.com/packages/3d/characters/humanoids/low-poly-animated-people-156748
For VFX I used EpicToonFX https://assetstore.unity.com/packages/vfx/particles/epic-toon-fx-57772 and WarFX https://assetstore.unity.com/packages/vfx/particles/war-fx-5669
For input control I used LeanTouch https://assetstore.unity.com/packages/tools/input-management/lean-touch-30111
For pooling LeanPool https://assetstore.unity.com/packages/tools/utilities/lean-pool-35666

_I will improve this project further mainly for practicing and portfolio purposes_
