using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlPo_hud_cl
{
    public class AlPo_hud_init : BaseScript
    {

        public Dictionary<int, string> directions = new Dictionary<int, string>
        {
            {0, "N"}, {45, "NW"}, {90, "W"}, {135, "SW"}, {180, "S"}, {225, "SE"}, {270, "E"}, {315, "NE"}, {360, "N"}
        };
        public int minutes;
        public int hours;
        public bool showHud = true;
        public double temp;
        public double temperature;
        public string direction1;

        public AlPo_hud_init()
        {
            Tick += firstTick;
            Tick += secondTick;
            API.RegisterCommand("hud", new Action<int>((source) =>
            {
                if (showHud)
                {
                    showHud = false;
                }
                else
                {
                    showHud = true;
                }
            }), false);
        }

        [Tick]
        public async Task firstTick()
        {
            if (!GetConfig.configLoaded)
            {
                return;
            }
            Vector3 coords = API.GetEntityCoords(API.PlayerPedId(), true, true);
            temp = Math.Floor(API.GetTemperatureAtCoords(coords.X, coords.Y, coords.Z) * 10) / 10;


            if (Convert.ToBoolean(GetConfig.Config["Fahrenheit"]))
            {
                temp = (temp * 9 / 5) + 32;
            }

            temperature = temp;

            foreach (var v in ClotheList.ClothesCats)
            {
                bool IsWearingClothes = Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), v);
                if (IsWearingClothes)
                {
                    if (Convert.ToBoolean(GetConfig.Config["Fahrenheit"]))
                    {
                        temperature = temperature + 1.8;
                    }
                    else
                    {
                        temperature = temperature + 1;
                    }
                }
            }

            temperature = Math.Floor(temperature * 10) / 10;

            foreach (var d in directions)
            {
                dynamic direction = API.GetEntityHeading(API.PlayerPedId());
                if (Math.Abs(direction - d.Key) < 22.5)
                {
                    direction1 = d.Value;
                    break;
                }
            }

            hours = API.GetClockHours();
            minutes = API.GetClockMinutes();
            string hora = hours.ToString();
            string minuto = minutes.ToString();

            if (hours <= 9)
            {
                hora = "0" + hora;
            }

            if (minutes <= 9)
            {
                minuto = "0" + minuto;
            }

            if (showHud)
            {
                if (Convert.ToBoolean(GetConfig.Config["Fahrenheit"]))
                {
                    if (temp < Convert.ToInt32(GetConfig.Config["ExtremeColdF"]))
                    {
                        await DrawTxt(GetConfig.Langs["zona"] + GetCurentTownName() + "~q~ - " + direction1 + " - " + GetConfig.Langs["dia"] + DayOfWeek() + "\n" + GetConfig.Langs["temp"] + "~COLOR_PLATFORM_BLUE~" + temperature.ToString() + "°F~q~" + " - " + GetConfig.Langs["hora"] + hora + ":" + minuto, GetConfig.Config["Text"]["Pos"][0].ToObject<float>(), GetConfig.Config["Text"]["Pos"][1].ToObject<float>(), 0.3f, 0.3f, 255, 255, 255, 255, true, false);
                    }
                    else if (temp < Convert.ToInt32(GetConfig.Config["ColdF"]) && temp >= Convert.ToInt32(GetConfig.Config["ExtremeColdF"]))
                    {
                        await DrawTxt(GetConfig.Langs["zona"] + GetCurentTownName() + "~q~ - " + direction1 + " - " + GetConfig.Langs["dia"] + DayOfWeek() + "\n" + GetConfig.Langs["temp"] + "~COLOR_BLUE~" + temperature.ToString() + "°F~q~" + " - " + GetConfig.Langs["hora"] + hora + ":" + minuto, GetConfig.Config["Text"]["Pos"][0].ToObject<float>(), GetConfig.Config["Text"]["Pos"][1].ToObject<float>(), 0.3f, 0.3f, 255, 255, 255, 255, true, false);
                    }
                    else if (temp < Convert.ToInt32(GetConfig.Config["CoolF"]) && temp >= Convert.ToInt32(GetConfig.Config["ColdF"]))
                    {
                        await DrawTxt(GetConfig.Langs["zona"] + GetCurentTownName() + "~q~ - " + direction1 + " - " + GetConfig.Langs["dia"] + DayOfWeek() + "\n" + GetConfig.Langs["temp"] + "~COLOR_BLUELIGHT~" + temperature.ToString() + "°F~q~" + " - " + GetConfig.Langs["hora"] + hora + ":" + minuto, GetConfig.Config["Text"]["Pos"][0].ToObject<float>(), GetConfig.Config["Text"]["Pos"][1].ToObject<float>(), 0.3f, 0.3f, 255, 255, 255, 255, true, false);
                    }
                    else if (temp < Convert.ToInt32(GetConfig.Config["WarmF"]) && temp >= Convert.ToInt32(GetConfig.Config["CoolF"]))
                    {
                        await DrawTxt(GetConfig.Langs["zona"] + GetCurentTownName() + "~q~ - " + direction1 + " - " + GetConfig.Langs["dia"] + DayOfWeek() + "\n" + GetConfig.Langs["temp"] + temperature.ToString() + "°F~q~" + " - " + GetConfig.Langs["hora"] + hora + ":" + minuto, GetConfig.Config["Text"]["Pos"][0].ToObject<float>(), GetConfig.Config["Text"]["Pos"][1].ToObject<float>(), 0.3f, 0.3f, 255, 255, 255, 255, true, false);
                    }
                    else if (temp < Convert.ToInt32(GetConfig.Config["HotF"]) && temp >= Convert.ToInt32(GetConfig.Config["WarmF"]))
                    {
                        await DrawTxt(GetConfig.Langs["zona"] + GetCurentTownName() + "~q~ - " + direction1 + " - " + GetConfig.Langs["dia"] + DayOfWeek() + "\n" + GetConfig.Langs["temp"] + "~COLOR_YELLOWSTRONG~" + temperature.ToString() + "°F~q~" + " - " + GetConfig.Langs["hora"] + hora + ":" + minuto, GetConfig.Config["Text"]["Pos"][0].ToObject<float>(), GetConfig.Config["Text"]["Pos"][1].ToObject<float>(), 0.3f, 0.3f, 255, 255, 255, 255, true, false);
                    }
                    else if (temp < Convert.ToInt32(GetConfig.Config["ExtremeHotF"]) && temp >= Convert.ToInt32(GetConfig.Config["HotF"]))
                    {
                        await DrawTxt(GetConfig.Langs["zona"] + GetCurentTownName() + "~q~ - " + direction1 + " - " + GetConfig.Langs["dia"] + DayOfWeek() + "\n" + GetConfig.Langs["temp"] + "~COLOR_ORANGE~" + temperature.ToString() + "°F~q~" + " - " + GetConfig.Langs["hora"] + hora + ":" + minuto, GetConfig.Config["Text"]["Pos"][0].ToObject<float>(), GetConfig.Config["Text"]["Pos"][1].ToObject<float>(), 0.3f, 0.3f, 255, 255, 255, 255, true, false);
                    }
                    else if (temp >= Convert.ToInt32(GetConfig.Config["ExtremeHotF"]))
                    {
                        await DrawTxt(GetConfig.Langs["zona"] + GetCurentTownName() + "~q~ - " + direction1 + " - " + GetConfig.Langs["dia"] + DayOfWeek() + "\n" + GetConfig.Langs["temp"] + "~COLOR_NET_PLAYER2~" + temperature.ToString() + "°F~q~" + " - " + GetConfig.Langs["hora"] + hora + ":" + minuto, GetConfig.Config["Text"]["Pos"][0].ToObject<float>(), GetConfig.Config["Text"]["Pos"][1].ToObject<float>(), 0.3f, 0.3f, 255, 255, 255, 255, true, false);
                    }
                }
                else
                {
                    if (temp < Convert.ToInt32(GetConfig.Config["ExtremeColdC"]))
                    {
                        await DrawTxt(GetConfig.Langs["zona"] + GetCurentTownName() + "~q~ - " + direction1 + " - " + GetConfig.Langs["dia"] + DayOfWeek() + "\n" + GetConfig.Langs["temp"] + "~COLOR_PLATFORM_BLUE~" + temperature.ToString() + "°C~q~" + " - " + GetConfig.Langs["hora"] + hora + ":" + minuto, GetConfig.Config["Text"]["Pos"][0].ToObject<float>(), GetConfig.Config["Text"]["Pos"][1].ToObject<float>(), 0.3f, 0.3f, 255, 255, 255, 255, true, false);
                    }
                    else if (temp < Convert.ToInt32(GetConfig.Config["ColdC"]) && temp >= Convert.ToInt32(GetConfig.Config["ExtremeColdC"]))
                    {
                        await DrawTxt(GetConfig.Langs["zona"] + GetCurentTownName() + "~q~ - " + direction1 + " - " + GetConfig.Langs["dia"] + DayOfWeek() + "\n" + GetConfig.Langs["temp"] + "~COLOR_BLUE~" + temperature.ToString() + "°C~q~" + " - " + GetConfig.Langs["hora"] + hora + ":" + minuto, GetConfig.Config["Text"]["Pos"][0].ToObject<float>(), GetConfig.Config["Text"]["Pos"][1].ToObject<float>(), 0.3f, 0.3f, 255, 255, 255, 255, true, false);
                    }
                    else if (temp < Convert.ToInt32(GetConfig.Config["CoolC"]) && temp >= Convert.ToInt32(GetConfig.Config["ColdC"]))
                    {
                        await DrawTxt(GetConfig.Langs["zona"] + GetCurentTownName() + "~q~ - " + direction1 + " - " + GetConfig.Langs["dia"] + DayOfWeek() + "\n" + GetConfig.Langs["temp"] + "~COLOR_BLUELIGHT~" + temperature.ToString() + "°C~q~" + " - " + GetConfig.Langs["hora"] + hora + ":" + minuto, GetConfig.Config["Text"]["Pos"][0].ToObject<float>(), GetConfig.Config["Text"]["Pos"][1].ToObject<float>(), 0.3f, 0.3f, 255, 255, 255, 255, true, false);
                    }
                    else if (temp < Convert.ToInt32(GetConfig.Config["WarmC"]) && temp >= Convert.ToInt32(GetConfig.Config["CoolC"]))
                    {
                        await DrawTxt(GetConfig.Langs["zona"] + GetCurentTownName() + "~q~ - " + direction1 + " - " + GetConfig.Langs["dia"] + DayOfWeek() + "\n" + GetConfig.Langs["temp"] + temperature.ToString() + "°C~q~" + " - " + GetConfig.Langs["hora"] + hora + ":" + minuto, GetConfig.Config["Text"]["Pos"][0].ToObject<float>(), GetConfig.Config["Text"]["Pos"][1].ToObject<float>(), 0.3f, 0.3f, 255, 255, 255, 255, true, false);
                    }
                    else if (temp < Convert.ToInt32(GetConfig.Config["HotC"]) && temp >= Convert.ToInt32(GetConfig.Config["WarmC"]))
                    {
                        await DrawTxt(GetConfig.Langs["zona"] + GetCurentTownName() + "~q~ - " + direction1 + " - " + GetConfig.Langs["dia"] + DayOfWeek() + "\n" + GetConfig.Langs["temp"] + "~COLOR_YELLOWSTRONG~" + temperature.ToString() + "°C~q~" + " - " + GetConfig.Langs["hora"] + hora + ":" + minuto, GetConfig.Config["Text"]["Pos"][0].ToObject<float>(), GetConfig.Config["Text"]["Pos"][1].ToObject<float>(), 0.3f, 0.3f, 255, 255, 255, 255, true, false);
                    }
                    else if (temp < Convert.ToInt32(GetConfig.Config["ExtremeHotC"]) && temp >= Convert.ToInt32(GetConfig.Config["HotC"]))
                    {
                        await DrawTxt(GetConfig.Langs["zona"] + GetCurentTownName() + "~q~ - " + direction1 + " - " + GetConfig.Langs["dia"] + DayOfWeek() + "\n" + GetConfig.Langs["temp"] + "~COLOR_ORANGE~" + temperature.ToString() + "°C~q~" + " - " + GetConfig.Langs["hora"] + hora + ":" + minuto, GetConfig.Config["Text"]["Pos"][0].ToObject<float>(), GetConfig.Config["Text"]["Pos"][1].ToObject<float>(), 0.3f, 0.3f, 255, 255, 255, 255, true, false);
                    }
                    else if (temp >= Convert.ToInt32(GetConfig.Config["ExtremeHotC"]))
                    {
                        await DrawTxt(GetConfig.Langs["zona"] + GetCurentTownName() + "~q~ - " + direction1 + " - " + GetConfig.Langs["dia"] + DayOfWeek() + "\n" + GetConfig.Langs["temp"] + "~COLOR_NET_PLAYER2~" + temperature.ToString() + "°C~q~" + " - " + GetConfig.Langs["hora"] + hora + ":" + minuto, GetConfig.Config["Text"]["Pos"][0].ToObject<float>(), GetConfig.Config["Text"]["Pos"][1].ToObject<float>(), 0.3f, 0.3f, 255, 255, 255, 255, true, false);
                    }
                }
            }
        }

        [Tick]
        private async Task secondTick()
        {
            await Delay(15000);
            if (!GetConfig.configLoaded)
            {
                return;
            }
            int ped = API.PlayerPedId();
            int stamina = API.GetAttributeCoreValue(ped, 1);
            if (Convert.ToBoolean(GetConfig.Config["Fahrenheit"]))
            {
                if (temp < Convert.ToInt32(GetConfig.Config["ExtremeColdF"]))
                {
                    Function.Call((Hash)0xC6258F41D86676E0, ped, 1, stamina - Convert.ToInt32(GetConfig.Config["LargeStaminaReduction"]));
                }
                else if (temp < Convert.ToInt32(GetConfig.Config["ColdF"]) && temp >= Convert.ToInt32(GetConfig.Config["ExtremeColdF"]))
                {
                    Function.Call((Hash)0xC6258F41D86676E0, ped, 1, stamina - Convert.ToInt32(GetConfig.Config["MediumStaminaReduction"]));
                }
                else if (temp < Convert.ToInt32(GetConfig.Config["CoolF"]) && temp >= Convert.ToInt32(GetConfig.Config["ColdF"]))
                {
                    Function.Call((Hash)0xC6258F41D86676E0, ped, 1, stamina - Convert.ToInt32(GetConfig.Config["LittleStaminaReduction"]));
                }
                else if (temp < Convert.ToInt32(GetConfig.Config["HotF"]) && temp >= Convert.ToInt32(GetConfig.Config["WarmF"]))
                {
                    TriggerEvent("vorpmetabolism:changeValue", "Thirst", Convert.ToInt32(GetConfig.Config["LittleWaterReduction"]));
                }
                else if (temp < Convert.ToInt32(GetConfig.Config["ExtremeHotF"]) && temp >= Convert.ToInt32(GetConfig.Config["HotF"]))
                {
                    TriggerEvent("vorpmetabolism:changeValue", "Thirst", Convert.ToInt32(GetConfig.Config["MediumWaterReduction"]));
                }
                else if (temp >= Convert.ToInt32(GetConfig.Config["ExtremeHotF"]))
                {
                    TriggerEvent("vorpmetabolism:changeValue", "Thirst", Convert.ToInt32(GetConfig.Config["LargeWaterReduction"]));
                }
            }
            else
            {
                if (temp < Convert.ToInt32(GetConfig.Config["ExtremeColdC"]))
                {
                    Function.Call((Hash)0xC6258F41D86676E0, ped, 1, stamina - Convert.ToInt32(GetConfig.Config["LargeStaminaReduction"]));
                }
                else if (temp < Convert.ToInt32(GetConfig.Config["ColdC"]) && temp >= Convert.ToInt32(GetConfig.Config["ExtremeColdC"]))
                {
                    Function.Call((Hash)0xC6258F41D86676E0, ped, 1, stamina - Convert.ToInt32(GetConfig.Config["MediumStaminaReduction"]));
                }
                else if (temp < Convert.ToInt32(GetConfig.Config["CoolC"]) && temp >= Convert.ToInt32(GetConfig.Config["ColdC"]))
                {
                    Function.Call((Hash)0xC6258F41D86676E0, ped, 1, stamina - Convert.ToInt32(GetConfig.Config["LittleStaminaReduction"]));
                }
                else if (temp < Convert.ToInt32(GetConfig.Config["HotC"]) && temp >= Convert.ToInt32(GetConfig.Config["WarmC"]))
                {
                    TriggerEvent("vorpmetabolism:changeValue", "Thirst", Convert.ToInt32(GetConfig.Config["LittleWaterReduction"]));
                }
                else if (temp < Convert.ToInt32(GetConfig.Config["ExtremeHotC"]) && temp >= Convert.ToInt32(GetConfig.Config["HotC"]))
                {
                    TriggerEvent("vorpmetabolism:changeValue", "Thirst", Convert.ToInt32(GetConfig.Config["MediumWaterReduction"]));
                }
                else if (temp >= Convert.ToInt32(GetConfig.Config["ExtremeHotC"]))
                {
                    TriggerEvent("vorpmetabolism:changeValue", "Thirst", Convert.ToInt32(GetConfig.Config["LargeWaterReduction"]));
                }
            }
        }

        private string DayOfWeek()
        {
            int dayOfWeek = API.GetClockDayOfWeek();

            if (dayOfWeek == 0) 
            {
                return GetConfig.Langs["domingo"];
            }
            else if (dayOfWeek == 1)
            {
                return GetConfig.Langs["lunes"];
            }
            else if (dayOfWeek == 2) 
            {
                return GetConfig.Langs["martes"];
            }
            else if (dayOfWeek == 3)
            {
                return GetConfig.Langs["miercoles"];
            }
            else if (dayOfWeek == 4)
            {
                return GetConfig.Langs["jueves"];
            }
            else if (dayOfWeek == 5)
            {
                return GetConfig.Langs["viernes"];
            }
            else
            {
                return GetConfig.Langs["sabado"];
            }
        }

        private string GetCurentTownName()
        {
            Vector3 pedCoords = API.GetEntityCoords(API.PlayerPedId(), true, true);
            uint town_hash = Function.Call<uint>((Hash)0x43AD8FC02B429D33, pedCoords.X, pedCoords.Y, pedCoords.Z, 1);
            switch (town_hash)
            {
                case 7359335:
                    return "~COLOR_GREEN~Annesburg";
                    break;
                case 3550472498:
                    return "~COLOR_GREEN~Armadillo";
                    break;
                case 1053078005:
                    return "~COLOR_GREEN~Blackwater";
                    break;
                case 2586580314:
                    return "~COLOR_GREEN~BeechersHope";
                    break;
                case 1862420670:
                    return "~COLOR_GREEN~Braithwaite";
                    break;
                case 1778899666:
                    return "~COLOR_GREEN~Caliga";
                    break;
                case 2443661614:
                    return "~COLOR_GREEN~Cornwall";
                    break;
                case 3821916002:
                    return "~COLOR_GREEN~Emerald";
                    break;
                case 406627834:
                    return "~COLOR_GREEN~Lagras";
                    break;
                case 1463094051:
                    return "~COLOR_GREEN~Manzanita";
                    break;
                case 2046780049:
                    return "~COLOR_GREEN~Rhodes";
                    break;
                case 3529426767:
                    return "~COLOR_GREEN~Saint Denis";
                    break;
                case 427683330:
                    return "~COLOR_GREEN~Strawberry";
                    break;
                case 2770008149:
                    return "~COLOR_GREEN~Tumbleweed";
                    break;
                case 459833523:
                    return "~COLOR_GREEN~Valentine";
                    break;
                case 2126321341:
                    return "~COLOR_GREEN~Vanhorn";
                    break;
                case 1663398575:
                    return "~COLOR_GREEN~Wapiti";
                    break;
                default:
                    return GetConfig.Langs["fuera_ciudad"];
                    break;
            }
        }

        public async Task DrawTxt(string text, float x, float y, float fontscale, float fontsize, int r, int g, int b, int alpha, bool textcentred, bool shadow)
        {
            long str = Function.Call<long>(Hash._CREATE_VAR_STRING, 10, "LITERAL_STRING", text);
            Function.Call(Hash.SET_TEXT_SCALE, fontscale, fontsize);
            Function.Call(Hash._SET_TEXT_COLOR, r, g, b, alpha);
            Function.Call(Hash.SET_TEXT_CENTRE, textcentred);
            if (shadow) { Function.Call(Hash.SET_TEXT_DROPSHADOW, 1, 0, 0, 255); }
            Function.Call(Hash.SET_TEXT_FONT_FOR_CURRENT_COMMAND, 1);
            Function.Call(Hash._DISPLAY_TEXT, str, x, y);
        }
    }
}
