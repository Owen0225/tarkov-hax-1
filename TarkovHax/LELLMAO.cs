using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EFT;
using FuckBoYShacks;
using System.Management;
using UnityEngine.Networking;
using System.Collections;

namespace FuckkBoyyyyyySHackkkHobo
{

    public class LELLMAO : MonoBehaviour
    {
        public LELLMAO()
        {
        }

        private GameObject GameObjectHolder;

        private IEnumerable<Player> _players;
        private IEnumerable<LootItem> _lootItems;
        private IEnumerable<ExfiltrationPoint> _extractPoints;

        private float _playersNextUpdateTime;
        private float _exfilNextUpdateTime;
        private float _espUpdateInterval = 10f;
        private float _itemNextUpdateTime;

        private bool _isESPMenuActive;
        private bool _isConfigMenuActive;
        private bool _showPlayersESP;
        private bool _crosshair;
        private bool _showItemESP;
        private bool _showValuables;
        private bool _showExfiljewEPS;
        private bool flag;
        private bool instantMagDrills;
        private bool _noRecoil;


        private float _maxDrawingDistance = 1500f;
        private float _maxLootDrawingDistance = 1500f;


        public void Load()
        {
            GameObjectHolder = new GameObject();
            GameObjectHolder.AddComponent<LELLMAO>();

            DontDestroyOnLoad(GameObjectHolder);
        }


        public void Unload()
        {
            Destroy(GameObjectHolder);
            Destroy(this);
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.End))
            {
                Unload();
            }
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                _isESPMenuActive = !_isESPMenuActive;

                if (_isConfigMenuActive)
                    _isConfigMenuActive = false;
            }
       
            if (Input.GetKey(KeyCode.KeypadPlus))
            {
                IncreaseFov();
            }
            if (Input.GetKey(KeyCode.KeypadMinus))
            {
                DecreaseFov();
            }

            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                EmuStrengthAndStamina();
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
               flag = !flag;
                foreach (Player player in ((IEnumerable<Player>)UnityEngine.Object.FindObjectsOfType<Player>()))
                {
                    if (player.isLocalPlayer || player.localPlayerAuthority)
                    {
                        if (flag)
                        {
                            player.PointOfView = EPointOfView.ThirdPerson;
                        }
                        if (!flag)
                        {
                            player.PointOfView = EPointOfView.FirstPerson;
                        }
                    }
                }
            }

            if (instantMagDrills)
            {
                foreach (Player player in this._players)
                {
                    if (player != null)
                    {
                        if (player.isLocalPlayer || player.localPlayerAuthority)
                        {
                            player.Skills.MagDrillsLoadSpeed.Value = 500f;
                            player.Skills.MagDrillsUnloadSpeed.Value = 500f;
                        }
                    }
                }
            }

            if (_noRecoil)
            {
                foreach (Player player in this._players)
                {
                    if (player.isLocalPlayer || player.localPlayerAuthority)
                    {
                        player.ProceduralWeaponAnimation.Shootingg.Intensity = 0f;
                        player.ProceduralWeaponAnimation.Shootingg.RecoilStrengthXY = new Vector2(0f, 0f);
                        player.ProceduralWeaponAnimation.Shootingg.RecoilStrengthZ = new Vector2(0f, 0f);
                    }
                }
            }

        }


        private void EmuStrengthAndStamina()
        {
            foreach (var ply in _players)
            {
               

                if (ply.isLocalPlayer || ply.localPlayerAuthority)
                {
                    ply.Physical.Sprinting.RestoreRate = 100.0f;
                    ply.Physical.Sprinting.DrainRate = 0.0f;
                    ply.Skills.StrengthBuffSprintSpeedInc.Value = 500f;

                }
            }
        }


        private void IncreaseFov()
        {
            Camera.main.fieldOfView += 1f;
        }

        private void DecreaseFov()
        {
            Camera.main.fieldOfView -= 1f;
        }



      


        private void OnGUI()
        {
            if (_isESPMenuActive)
            {
                DrawESPMenu();
            }

            if (_isConfigMenuActive)
            {
                DrawConfigMenu();
            }


            GUI.Label(new Rect(10f, 10f, 1000f, 500), "<COLOR=#FF0000>Z</color><COLOR=#FF4600>e</color><COLOR=#FF8C00>u</color><COLOR=#FFD200>s</color><COLOR=#FFff00></color><COLOR=#B9ff00>C</color><COLOR=#73ff00>o</color><COLOR=#2Dff00></color><COLOR=#00ff00>d</color><COLOR=#00ff46></color><COLOR=#00ff8C>e</color>");

            if (_showPlayersESP && Time.time >= _playersNextUpdateTime)
            {
                _players = FindObjectsOfType<Player>();
                _playersNextUpdateTime = Time.time + _espUpdateInterval;
            }

        

            if (_showPlayersESP)
            {
                DrawPlayers();
            }


            if (_showExfiljewEPS && Time.time >= _exfilNextUpdateTime)
            {
                _extractPoints = FindObjectsOfType<ExfiltrationPoint>();
                _exfilNextUpdateTime = Time.time + _espUpdateInterval;
            }

            if (_showExfiljewEPS)
            {
                jewExfilEPS();
            }

            if (_showItemESP && Time.time >= _itemNextUpdateTime)
            {
                _lootItems = FindObjectsOfType<LootItem>();
                _itemNextUpdateTime = Time.time + _espUpdateInterval;
            }
            if (_showItemESP && !_showValuables)
            {
                DrawLoot();
            }
            if (_showItemESP && _showValuables)
            {
                DrawValuableLoot();
            }

            if (_crosshair)
            {
                DrawCrosshair();
            }
        }

        public void DrawCrosshair()
        {
           GuiHelper.DrawBox((float)Screen.width / 2f, (float)Screen.height / 2f - 5f, 1f, 11f, Color.yellow);
           GuiHelper.DrawBox((float)Screen.width / 2f - 5f, (float)Screen.height / 2f, 11f, 1f, Color.yellow);
        }

        private void jewExfilEPS()
        {
            foreach (var point in _extractPoints)
            {
                if (point != null)
                {
                    float distanceToObject = Vector3.Distance(Camera.main.transform.position, point.transform.position);
                    var exfilContainerBoundingVector = new Vector3(
                        Camera.main.WorldToScreenPoint(point.transform.position).x,
                        Camera.main.WorldToScreenPoint(point.transform.position).y,
                        Camera.main.WorldToScreenPoint(point.transform.position).z);

                    if (exfilContainerBoundingVector.z > 0.01)
                    {
                        GUI.color = Color.green;
                        int distance = (int)distanceToObject;
                        String exfilName = point.name;
                        string boxText = $"{exfilName} - {distance}m";

                        GUI.Label(new Rect(exfilContainerBoundingVector.x - 50f, (float)Screen.height - exfilContainerBoundingVector.y, 100f, 50f), boxText);
                    }
                }
            }
        }

        public void DrawLoot()
        {
            foreach (LootItem item in _lootItems)
            {
                if (item == null)
                {
                    break;
                }
                float num = Vector3.Distance(Camera.main.transform.position, item.transform.position);
                Vector3 vector = new Vector3(Camera.main.WorldToScreenPoint(item.transform.position).x, Camera.main.WorldToScreenPoint(item.transform.position).y, Camera.main.WorldToScreenPoint(item.transform.position).z);
                if ((double)vector.z > 0.01)
                {
                    GUI.color = Color.cyan;
                    int num2 = (int)num;
                    string name = item.name;
                    string text = string.Format("{0} - {1}m", name, num2);
                    GUI.Label(new Rect(vector.x - 50f, (float)Screen.height - vector.y, 100f, 50f), text);
                }
            }
        }

        public void DrawValuableLoot()
        {
            foreach (LootItem lootItem in _lootItems)
            {
                if (lootItem == null)
                {
                    break;
                }
                if (lootItem.name == null)
                {
                    break;
                }
                if (lootItem.name == string.Empty)
                {
                    break;
                }
                if (lootItem.name.Contains("powder") || lootItem.name.Contains("roler") || lootItem.name.Contains("lion") || lootItem.name.Contains("cat") || lootItem.name.Contains("gas") || lootItem.name.Contains("bitcoin") || lootItem.name.Contains("video") || lootItem.name.Contains("item_chain_gold") || lootItem.name.Contains("cpu") || lootItem.name.Contains("gphone") || lootItem.name.Contains("supressor") || lootItem.name.Contains("gmcount") || lootItem.name.Contains("gm") || lootItem.name.Contains("weapon_remington_r11_rsass") || lootItem.name.Contains("weapon_colt_m4a1_556x45"))
                {
                    float num = Vector3.Distance(Camera.main.transform.position, lootItem.transform.position);
                    Vector3 vector = new Vector3(Camera.main.WorldToScreenPoint(lootItem.transform.position).x, Camera.main.WorldToScreenPoint(lootItem.transform.position).y, Camera.main.WorldToScreenPoint(lootItem.transform.position).z);
                    if (num <= _maxLootDrawingDistance && (double)vector.z > 0.01)
                    {
                        GUI.color = Color.cyan;
                        int num2 = (int)num;
                        string name = lootItem.name;
                        string text = string.Format("{0} - {1}m", name, num2);
                        GUI.Label(new Rect(vector.x - 50f, (float)Screen.height - vector.y, 100f, 50f), text);
                    }
                }
            }
        }


        public void DrawPlayers()
        {
            foreach (Player player in _players)
            {
                if (player == null)
                {
                    break;
                }
                float num = Vector3.Distance(Camera.main.transform.position, player.Transform.position);
                Vector3 vector = new Vector3(Camera.main.WorldToScreenPoint(player.Transform.position).x, Camera.main.WorldToScreenPoint(player.Transform.position).y, Camera.main.WorldToScreenPoint(player.Transform.position).z);
                if (num <= _maxDrawingDistance && (double)vector.z > 0.01)
                {
                    Vector3 vector2 = new Vector3(Camera.main.WorldToScreenPoint(player.PlayerBones.Head.position).x, Camera.main.WorldToScreenPoint(player.PlayerBones.Head.position).y, Camera.main.WorldToScreenPoint(player.PlayerBones.Head.position).z);
                    float x = Camera.main.WorldToScreenPoint(player.Transform.position).x;
                    float num2 = Camera.main.WorldToScreenPoint(player.PlayerBones.Head.position).y + 10f;
                    float num3 = Math.Abs(Camera.main.WorldToScreenPoint(player.PlayerBones.Head.position).y - Camera.main.WorldToScreenPoint(player.Transform.position).y) + 10f;
                    float num4 = num3 * 0.65f;
                    Color playerColor = GetPlayerColor(player.Side);
                    bool flag = player.Profile.Info.RegistrationDate <= 0;
                    Color color = player.Profile.Health.IsAlive ? playerColor : Color.gray;
                    GUI.color = color;
                    GuiHelper.DrawBox(x - num4 / 2f, (float)Screen.height - num2, num4, num3, color);
                    GuiHelper.DrawLine(new Vector2(vector2.x - 2f, (float)Screen.height - vector2.y), new Vector2(vector2.x + 2f, (float)Screen.height - vector2.y), color);
                    GuiHelper.DrawLine(new Vector2(vector2.x, (float)Screen.height - vector2.y - 2f), new Vector2(vector2.x, (float)Screen.height - vector2.y + 2f), color);
                    string text = flag ? "AI" : player.Profile.Info.Nickname;
                    float num5 = player.HealthController.SummaryHealth.CurrentValue / 435f * 100f;
                    string arg = player.Profile.Health.IsAlive ? text : (text + " (DEAD)");
                    if (player.Profile.Health.IsAlive)
                    {
                        string text2 = "Hatchet/Knife";
                        try
                        {
                            if (player.Weapon != null && player.Weapon.Template != null && player.Weapon.Template.ShortName != null && player.Profile.Health.IsAlive)
                            {
                                text2 = player.Weapon.Template.ShortName;
                            }
                        }
                        catch
                        {
                        }
                        GUI.color = Color.yellow;
                        GUI.skin.GetStyle(text2).CalcSize(new GUIContent(text2));
                        Vector2 vector3 = GUI.skin.GetStyle(text2).CalcSize(new GUIContent(text2));
                        GUI.Label(new Rect(vector.x - vector3.x / 2f, (float)Screen.height - num2 - 40f, 300f, 50f), text2);
                    }
                    GUI.color = color;
                    string text3 = string.Format("[{0}%] {1} [{2}m]", (int)num5, arg, (int)num);
                    GUI.skin.GetStyle(text3).CalcSize(new GUIContent(text3));
                    Vector2 vector4 = GUI.skin.GetStyle(text3).CalcSize(new GUIContent(text3));
                    GUI.Label(new Rect(vector.x - vector4.x / 2f, (float)Screen.height - num2 - 20f, 300f, 50f), text3);
                }
            }
        }



        private Color GetPlayerColor(EPlayerSide side)
        {
            switch (side)
            {
                case EPlayerSide.Bear:
                    return Color.red;
                case EPlayerSide.Usec:
                    return Color.blue;
                case EPlayerSide.Savage:
                    return Color.white;
                default:
                    return Color.white;
            }
        }

        //private void DrawESPMenu()
        //{
        //    GUI.color = Color.black;
        //    GUI.Box(new Rect(100f, 100f, 190f, 190f), "");

        //    GUI.color = Color.white;
        //    GUI.Label(new Rect(180f, 110f, 150f, 20f), "REE");

        //    _showPlayersESP = GUI.Toggle(new Rect(110f, 140f, 120f, 20f), _showPlayersESP, "  Players");
        //    _showExfiljewEPS = GUI.Toggle(new Rect(110f, 180f, 120f, 20f), _showExfiljewEPS, "  Exit ESP");
        //}

        public void DrawESPMenu()
        {
            GUI.color = Color.black;
            GUI.Box(new Rect(100f, 100f, 190f, 400f), "");
            GUI.color = Color.white;
            GUI.Label(new Rect(180f, 110f, 150f, 20f), "SC 1.0");
            _showPlayersESP = GUI.Toggle(new Rect(110f, 140f, 120f, 20f), _showPlayersESP, "  Players");
            _showExfiljewEPS = GUI.Toggle(new Rect(110f, 160f, 120f, 20f), _showExfiljewEPS, "  Exit ESP");
            _noRecoil = GUI.Toggle(new Rect(110f, 180f, 120f, 20f), _noRecoil, "  No-Recoil");
            _crosshair = GUI.Toggle(new Rect(110f, 200f, 120f, 20f), _crosshair, "  Crosshair");
            instantMagDrills = GUI.Toggle(new Rect(110f, 220f, 120f, 30f), instantMagDrills, "  Instant Mag Drills");
            _isConfigMenuActive = GUI.Toggle(new Rect(110f, 240f, 120f, 20f), _isConfigMenuActive, " Config Menu");
            _showItemESP = GUI.Toggle(new Rect(110f, 260f, 120f, 20f), _showItemESP, " Loot ESP");
        }

        private void DrawConfigMenu()
        {
            GUI.color = Color.black;
            GUI.Box(new Rect(400f, 100f, 190f, 400f), "");
            GUI.color = Color.white;
            GUI.Label(new Rect(480f, 110f, 150f, 20f), "Config");
            GUI.Label(new Rect(410f, 140f, 150f, 20f), "ESP Distance: " + _maxDrawingDistance);
            _maxDrawingDistance = GUI.HorizontalSlider(new Rect(410f, 170f, 150f, 20f), _maxDrawingDistance, 0f, 1500f);
            GUI.Label(new Rect(410f, 200f, 150f, 20f), "Loot Distance: " + _maxLootDrawingDistance);
            _maxLootDrawingDistance = GUI.HorizontalSlider(new Rect(410f, 230f, 150f, 20f), _maxLootDrawingDistance, 0f, 1500f);
            _showValuables = GUI.Toggle(new Rect(410f, 260f, 150f, 20f), _showValuables, "  Valuables ONLY");
        }

        private double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2.0) + Math.Pow(y2 - y1, 2.0));
        }
    }
}
