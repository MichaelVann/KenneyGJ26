using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public static class VLib
{
    static string[] m_consonants = { "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "r", "s", "t", "v", "w", "z", };// "x", "q", "ch", "kh", "gh" };
    static string[] m_vowels = { "a", "e", "i", "o", "u" };//, "oo", "ou", "ee" };

    internal static Color COLOR_yellow = new Color(1f,188f/255f,0f);
    public static string GetEnumName<T>(T a_type) { return Enum.GetName(typeof(T), a_type); }

    public const float _degreesToRadians = 3.14159265358979323f / 180f;

    public const float _celciusToKelvin = 273.15f;

    public const float _msToKmh = 3.6f;
    public const float _msToMph = 2.23694f;

    public static string FirstLetterToUpperCaseOrConvertNullToEmptyString(this string s)
    {
        if (string.IsNullOrEmpty(s))
            return string.Empty;

        char[] a = s.ToCharArray();
        a[0] = char.ToUpper(a[0]);
        return new string(a);
    }

    public static float TruncateFloatsDecimalPlaces(float a_float, int a_decimalsToKeep)
    {
        float multiplier = Mathf.Pow(10, a_decimalsToKeep);
        float truncatedFloat = ((int)(a_float * multiplier) / multiplier);

        return truncatedFloat;
    }

    #region Clamps

    public static float BigClamp(float a_value, float a_ceiling)
    {
        return a_value > a_ceiling ? a_ceiling : a_value;
    }

    public static int BigClamp(int a_value, int a_ceiling)
    {
        return a_value > a_ceiling ? a_ceiling : a_value;
    }

    public static double BigClamp(double a_value, double a_ceiling)
    {
        return a_value > a_ceiling ? a_ceiling : a_value;
    }

    public static float LilClamp(float a_value, float a_floor)
    {
        return a_value < a_floor ? a_floor : a_value;
    }

    public static int LilClamp(int a_value, int a_floor)
    {
        return a_value < a_floor ? a_floor : a_value;
    }

    public static double LilClamp(double a_value, double a_floor)
    {
        return a_value < a_floor ? a_floor : a_value;
    }

    #endregion

    public static float SigmoidLerp(float a_start, float a_finish, float a_t, int a_sensitivity = 3)
    {
        float retVal = 0f;
        float x = a_t;
        int y = a_sensitivity;
        if (a_t <= 0.5)
        {
            retVal = Mathf.Pow(x, (float)y) *(float)(2 << ((int)y - 2));
        }
        else
        {
            retVal = (float)(Mathf.Pow(-2, y - 1)) * Mathf.Pow(x - 1f, (float)y) + 1f;
        }

        retVal = a_start + (a_finish - a_start) * retVal;

        return retVal;
    }

    public static Vector2 SigmoidLerp(Vector2 a_start, Vector3 a_finish, float a_t, int a_sensitivity = 3)
    {
        Vector2 retVec = new Vector2();
        retVec.x = SigmoidLerp(a_start.x, a_finish.x, a_t, a_sensitivity);
        retVec.y = SigmoidLerp(a_start.y, a_finish.y, a_t, a_sensitivity);
        return retVec;
    }

    public static Vector3 SigmoidLerp(Vector3 a_start, Vector3 a_finish, float a_t, int a_sensitivity = 3)
    {
        Vector3 retVec = new Vector3();
        retVec.x = SigmoidLerp(a_start.x, a_finish.x, a_t, a_sensitivity);
        retVec.y = SigmoidLerp(a_start.y, a_finish.y, a_t, a_sensitivity);
        retVec.z = SigmoidLerp(a_start.z, a_finish.z, a_t, a_sensitivity);
        return retVec;
    }

    public static float Eerp(float a_start, float a_end, float a_time, float a_exponent)
    {
        float retVal = Mathf.Lerp(a_start,a_end,Mathf.Pow(a_time,a_exponent));
        return retVal;
    }

    public static float ClampRotation(float a_rotation)
    {
        float result = a_rotation;
        bool modified = true;
        while (modified)
        {
            modified = false;
            if (result > 180f)
            {
                result -= 360f;
                modified = true;
            }
            else if (result < -180f)
            {
                result += 360f;
                modified = true;
            }
        }
        return result;
    }

    public static Vector2 EulerAngleToVector2(float a_angle)
    {
        float x = Mathf.Sin(-a_angle * Mathf.Deg2Rad);
        float y = Mathf.Cos(a_angle * Mathf.Deg2Rad);
        return new Vector2(x, y);
    }

    public static Vector3 Euler2dAngleToVector3(float a_angle)
    {
        float x = -Mathf.Sin(a_angle * Mathf.PI / 180f);
        float y = Mathf.Cos(a_angle * Mathf.PI / 180f);
        float z = 0f;
        return new Vector3(x, y, z);
    }

    public static Quaternion Vector2DirectionToQuaternion(Vector2 a_vector2)
    {
        return Quaternion.Euler(0f,0f,Vector2ToEulerAngle(a_vector2));
    }

    public static Vector2 RotateVector2(this Vector2 a_vector2, float a_angle)
    {
        return RotateVector3In2D(a_vector2, a_angle);
    }

    public static Vector3 RotateVector3In2D(this Vector3 a_vector3, float a_angle)
    {
        return Quaternion.AngleAxis(a_angle, Vector3.forward) * a_vector3;
    }
    public static Vector2 ToVector2(this Vector3 a_vector3)
    {
        return new Vector2(a_vector3.x, a_vector3.y);
    }

    public static Vector3 ToVector3(this Vector2 a_vector3)
    {
        return new Vector3(a_vector3.x, a_vector3.y,0f);
    }

    public static Vector3 HadamardProduct(this Vector3 a_thisVector, Vector3 a_otherVector)
    {
        return new Vector3(a_thisVector.x * a_otherVector.x, a_thisVector.y * a_otherVector.y, a_thisVector.z * a_otherVector.z);
    }

    public static float AngleBetweenVector3s(Vector3 a_vectorA, Vector3 a_vectorB)
    {
        float angle = 0f;
        
        return angle;
    }

    public static float AngleBetweenVector2s(Vector2 a_vectorA, Vector2 a_vectorB)
    {
        float deltaAngle = 0f;

        float angleA = Vector2ToEulerAngle(a_vectorA);
        float angleB = Vector2ToEulerAngle(a_vectorB);

        deltaAngle = ClampRotation(angleA - angleB);
        return deltaAngle;
    }

    public static float Vector2ToEulerAngle(Vector2 a_vector2)
    {
        float angle = Vector2.SignedAngle(new Vector2(0f,1f), a_vector2);
        return angle;
    }

    public static Vector3 Vector2ToEulerAngles(Vector2 a_vector2)
    {
        return new Vector3(0f, 0f, Vector2ToEulerAngle(a_vector2));
    }

    public static Vector2 RandomVector2Direction()
    {
        return new Vector2(vRandom(-1f, 1f), vRandom(-1f, 1f)).normalized;
    }

    public static float Vector3ToEulerAngle(Vector3 a_vector3)
    {
        return Vector2ToEulerAngle(a_vector3.ToVector2());
    }

    public static Vector3 Vector3ToEulerAngles(Vector3 a_vector3)
    {
        return Vector2ToEulerAngles(a_vector3.ToVector2());
    }

    public static Quaternion Vector3ToQuaternion(Vector3 a_vector3)
    {
        return Quaternion.Euler(Vector3ToEulerAngles(a_vector3));
    }

    public static Vector3 RandomVector3Direction()
    {
        return new Vector3(vRandom(-1f,1f), vRandom(-1f, 1f), vRandom(-1f, 1f)).normalized;
    }

    public static float FindDeltaAngle(float a_currentAngle ,float a_desiredAngle)
    {
        float result = 0f;
        result = a_desiredAngle - a_currentAngle;

        while (result > 180f)
        {
            result -= 360f;
        }

        while (result < -180f)
        {
            result += 360f;
        }

        return result;
    }

    public static float FindDeltaAngle(Vector2 a_from, Vector2 a_to)
    {
        return FindDeltaAngle(Vector2ToEulerAngle(a_from), Vector2ToEulerAngle(a_to));
    }

    public static float CosDeg(float a_angle)
    {
        return Mathf.Cos(a_angle * Mathf.PI/180f);
    }

    public static int SafeMod(int a_value, int a_mod)
    {
        return (a_value % a_mod + a_mod) % a_mod;
    }

    public static float SafeMod(float a_value, int a_mod)
    {
        return (a_value % a_mod + a_mod) % a_mod;
    }

    public static int vRandom(int a_inclusiveMin, int a_inclusiveMax)
    {
        return UnityEngine.Random.Range(a_inclusiveMin, a_inclusiveMax+1);
    }

    public static float vRandom(float a_inclusiveMin, float a_inclusiveMax)
    {
        return UnityEngine.Random.Range(a_inclusiveMin, a_inclusiveMax);
    }

    public static Vector2 vRandom(Vector2 a_inclusiveMin, Vector2 a_inclusiveMax)
    {
        return new Vector2(vRandom(a_inclusiveMin.x, a_inclusiveMax.x), vRandom(a_inclusiveMin.y, a_inclusiveMax.y));
    }

    public static Vector3 ClampVector3(Vector3 a_vector, Vector3 a_min, Vector3 a_max)
    {
        Vector3 clampedVector = a_vector;
        clampedVector.x = Mathf.Clamp(clampedVector.x, a_min.x, a_max.x);
        clampedVector.y = Mathf.Clamp(clampedVector.y, a_min.y, a_max.y);
        clampedVector.z = Mathf.Clamp(clampedVector.z, a_min.z, a_max.z);

        return clampedVector;
    }

    public static float[] vRandomSpread(int a_count,float a_deviation)
    {
        float[] rolls = new float[a_count];

        float totalRoll = 0f;
        for (int i = 0; i < rolls.Length; i++)
        {
            rolls[i] += vRandom(0f, 100f);
            totalRoll += rolls[i];
        }

        for (int i = 0; i < rolls.Length; i++)
        {
            rolls[i] = rolls[i] / (totalRoll/2f);
            rolls[i] = (1f - a_deviation) + a_deviation * rolls[i];
        }

        return rolls;
    }

    public static Color Randomise(this Color a_color)
    {
        Color newColor = new Color();
        newColor.r = VLib.vRandom(0f, 1f);
        newColor.g = VLib.vRandom(0f, 1f);
        newColor.b = VLib.vRandom(0f, 1f);
        newColor.a = 1f;

        return newColor;
    }

    public static int ChanceTruncate(float a_input)
    {
        return (int)a_input + (vRandom(0f, 1f) <= a_input % 1f ? 1 : 0);
    }

    public static float PowPreserveSign(float a_value, float a_exponent)
    {
        float result = 0f;

        result = Mathf.Abs(Mathf.Pow(a_value, a_exponent)) * (a_value >= 0 ? 1f : -1f);

        return result;
    }

    public static List<T> RemoveLast<T>(this List<T> a_list)
    {
        a_list.RemoveAt(a_list.Count - 1);
        return a_list;
    }

    //Returns the color with specified alpha
    public static Color WithAlpha(this Color a_color, float a_alpha)
    {
        Color newColor = a_color;
        newColor.a = a_alpha;

        return newColor;
    }

    public static Color RatioToColorRYW(float a_ratio)
    {
        Color returnColor = Color.white;

        float t = 0f;

        if (a_ratio <= 1f / 2f)
        {
            t = a_ratio * 2f;
            returnColor = Color.Lerp(Color.red, Color.yellow, t);
        }
        else
        {
            t = (a_ratio - 0.5f) * 2f;
            returnColor = Color.Lerp(Color.yellow, Color.white, t);
        }

        return returnColor;
    }

    public static Color RatioToColorRGB(float a_ratio)
    {
        Color returnColor = Color.white;

        float redRatio = Mathf.Clamp(2f - (2f * a_ratio), 0f, 1f);
        float greenRatio = Mathf.Clamp(2f*a_ratio, 0f, 1f);
        returnColor = new Color(redRatio, greenRatio, 0f);
        return returnColor;
    }

    public static Color RatioToColorRYWB(float a_ratio)
    {
        Color returnColor = Color.white;

        if (a_ratio <= 1/3f)
        {
            returnColor = Color.Lerp(Color.red, Color.yellow, a_ratio * 3f);
        }
        else if (a_ratio <= 2 / 3f)
        {
            returnColor = Color.Lerp(Color.yellow, Color.white, a_ratio % (1/3f) * 3f);
        }
        else
        {
            returnColor = Color.Lerp(Color.white, Color.black, a_ratio % (1/3f) * 3f);
        }

        return returnColor;
    }

    internal static Color RatioToColorRarity(float a_ratio)
    {
        Color returnColor = Color.white;
        float cases = 6f;
        float mod = (1f / cases);
        float modRatio = a_ratio % mod;
        modRatio /= mod;
        switch (a_ratio)
        {
            case float n when (n >= 0f / cases && n < 1f/ cases):
                returnColor = RatioToColorRGB(modRatio);// (1f-a_ratio) / (1f / cases));
                break;
            case float n when (n >= 1f / cases && n < 2f / cases):
                returnColor = Color.Lerp(Color.green, Color.cyan, modRatio);
                break;
            case float n when (n >= 2f / cases && n < 3f / cases):
                returnColor = Color.Lerp(Color.cyan, Color.blue, modRatio);
                break;
            case float n when (n >= 3f / cases && n < 4f / cases):
                returnColor = Color.Lerp(Color.blue, Color.magenta, modRatio);
                break;
            case float n when (n >= 4f / cases && n < 5f / cases):
                returnColor = Color.Lerp(Color.magenta, Color.white, modRatio);
                break;
            case float n when (n >= 5f / cases && n < 6f / cases):
                returnColor = Color.Lerp(Color.white, Color.black, modRatio);
                break;
            default:
                break;
        }

        return returnColor;
    }

    public static Color RatioToColorRGBWithAlpha(float a_percentage)
    {
        Color returnColor = RatioToColorRGB(a_percentage);

        returnColor.a = a_percentage;// = new Color(redRatio, greenRatio, 0f, a_percentage);
        return returnColor;
    }

    public static string GenerateRandomizedName(int a_minimumLength = 2, int a_maximumLength = 10)
    {
        string name = "";
        int length = vRandom(a_minimumLength, a_maximumLength);
        int startWithVowel = vRandom(0, 1);

        for (int i = 0; i < length; i++)
        {
            if (i % 2 == startWithVowel)
            {
                name += m_consonants[vRandom(0, m_consonants.Length-1)];
            }
            else
            {
                name += m_vowels[vRandom(0, m_vowels.Length-1)];
            }
            if (i == 0)
            {
                string tempString = name;
                tempString = name[0].ToString().ToUpper();
                for (int j = 1; j < name.Length; j++)
                {
                    tempString += name[i];
                }
                name = tempString;
            }
        }
        return name;
    }

    //Returns a string[2]
    public static string[] GenerateRandomPersonsName()
    {
        string[] generatedNames = new string[2];

        string[] firstNames =
        {
            "Andrew",
            "Bart",
            "Ben",
            "John",
            "Stuart",
            "Giles",
            "Carnaby",
            "Sean",
            "Will",
            "Micky",
            "Jack",
            "Tyler",
            "Israq",
            "Daniel",
            "Stephen",
            "Richard",
            "Oliver",
            "Archie",

            "Niamh",
            "Eloise",
            "Mikenna",
            "Molly",
            "Emma",
            "Lavinia",
            "Sandra",
            "Mary",
            "Julia",
            "Tina",
            "Penelope",
            "Lucy",
            "Becky",
            "Ruth",
            "Calli",
            "Aditi",
            "Molly",
            "Emma",
            "Naomi",
            "Zara",
        };

        string[] lastNames =
        {
            "Digby",
            "Webster-Hawes",
            "Constant",
            "Vann",
            "Ison",
            "Hansen",
            "Gilani",
            "Dhrubo",
            "Modra",
            "Head",
            "Hensgen",
            "Basnet",
            "Shoebridge",
            "Porter",
            "Foster",
            "Erieau",
            "Gordon-Anderson",
            "Taylor",
            "Waters"
        };

        generatedNames[0] = firstNames[UnityEngine.Random.Range(0, firstNames.Length)];
        generatedNames[1] = lastNames[UnityEngine.Random.Range(0, lastNames.Length)];

        return generatedNames;
    }

    public static string[] GetEnumNames(Type enumType)
    {
        if (!enumType.IsEnum)
        {
            throw new ArgumentException("The provided type is not an enum.");
        }

        return Enum.GetNames(enumType);
    }

    public static float RoundToDecimalPlaces(float a_value, int a_decimalPlaces)
    {
        float result = a_value;
        float shift = Mathf.Pow(10f, a_decimalPlaces);
        result *= shift;
        result = Mathf.Round(result);
        result /= shift;
        return result;
    }

    public static float RoundDownToDecimalPlaces(float a_value, int a_decimalPlaces)
    {
        float result = a_value;
        float shift = Mathf.Pow(10f, a_decimalPlaces);
        result *= shift;
        result = Mathf.Floor(result);
        result /= shift;
        return result;
    }

    public static string ScrambleAlphabeticalString(string a_input)
    {
        string output = string.Empty;
        int charBase = -1;
        int cypher = 1; //VLib.vRandom(0, 26);
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < a_input.Length; i++)
        {
            char c = a_input[i];
            if (c != ' ')
            {
                float roll = VLib.vRandom(0f, 1f);
                if (roll >= 0.5)
                {
                    c = '?';
                }
                else if (roll >= 0.25)
                {
                    int symbolRoll = vRandom(1, 4);
                    switch (symbolRoll)
                    {
                        case 1:
                            c = '!';
                            break;
                        case 2:
                            c = '#';
                            break;
                        case 3:
                            c = '&';
                            break;
                        case 4:
                            c = '@';
                            break;
                    }
                }
                else
                {
                    if (c > 91)
                    {
                        charBase = 97;
                    }
                    else
                    {
                        charBase = 65;
                    }
                    int cInt = c + cypher;
                    cInt = (cInt - charBase) % 26;
                    c = (char)(cInt + charBase);
                }

            }
            builder.Append(c);
        }
        output = builder.ToString();
        return output;
    }

    public static char ScrambleAlphabeticalCharacter(char a_char)
    {
        if (a_char != ' ')
        {
            int charBase = -1;
            int cypher = 1;
            float roll = VLib.vRandom(0f, 1f);
            if (roll >= 0.5)
            {
                a_char = '?';
            }
            else if (roll >= 0.25)
            {
                int symbolRoll = vRandom(1, 4);
                switch (symbolRoll)
                {
                    case 1:
                        a_char = '!';
                        break;
                    case 2:
                        a_char = '#';
                        break;
                    case 3:
                        a_char = '&';
                        break;
                    case 4:
                        a_char = '@';
                        break;
                }
            }
            else
            {
                if (a_char > 91)
                {
                    charBase = 97;
                }
                else
                {
                    charBase = 65;
                }
                int cInt = a_char + cypher;
                cInt = (cInt - charBase) % 26;
                a_char = (char)(cInt + charBase);
            }
        }
        return a_char;
    }

    public static string ScrambleRandomAlphabeticalCharacter(string a_input)
    {
        string output = string.Empty;
        if(a_input.Length > 0)
        {
            StringBuilder builder = new StringBuilder(a_input);
            int index = VLib.vRandom(0, a_input.Length - 1);
            builder[index] = ScrambleAlphabeticalCharacter(a_input[index]);
            output = builder.ToString();
        }
        return output;
    }

    internal static bool IsCharCapital(char a_char)
    {
        bool result = false;
        result = (a_char >= 'A' && a_char <= 'Z');
        return result;
    }

    internal static int CharToInt(char a_char)
    {
        int result = a_char - '0';
        return result;
    }

    internal static int StringToInt(string a_string)
    {
        int result = 0;
        int powerOfTen = 0;
        for (int i = a_string.Length-1; i >= 0; i--)
        {
            result += (int)(CharToInt(a_string[i]) * Math.Pow(10, powerOfTen));
            powerOfTen++;
        }
        return result;
    }

    internal static string CleanUpString(string a_string)
    {
        string result = string.Empty;

        for (int i = 0; i < a_string.Length; i++)
        {
            if (i != 0 && IsCharCapital(a_string[i]))
            {
                result += ' ';
            }
            result += a_string[i];
        }

        return result;
    }

    internal static int[] GetApplicationVersionNumbers()
    {
        int[] versionNumbers = new int[2];
        //versionNumbers[0] = int.Parse(Application.version);
        string versionString = Application.version;
        for (int i = versionString.Length-1; i >= 0; i--)
        {
            if (versionString[i] == '.')
            {
                versionNumbers[0] = StringToInt(versionString.Substring(0, i));
                break;
            }
        }


        int substringIndex = -1;
        for (int i = 0; i < versionString.Length; i++)
        {
            if (versionString[i] == '.')
            {
                substringIndex = i + 1;
                break;
            }
        }
        versionNumbers[1] = StringToInt(versionString.Substring(substringIndex));
        return versionNumbers;
    }

    static internal void DrawSpriteBetween2Points(SpriteRenderer a_spriteRenderer, Vector3 a_pointA, Vector3 a_pointB)
    {
        float textureHeight = a_spriteRenderer.sprite.texture.height;
        float pixelsPerUnit = a_spriteRenderer.sprite.pixelsPerUnit;

        Vector3 distanceVector = a_pointB - a_pointA;

        float desiredHeight = distanceVector.magnitude * pixelsPerUnit / textureHeight;
        float currentHeight = a_spriteRenderer.bounds.size.y;
        float scale = desiredHeight / currentHeight;

        a_spriteRenderer.transform.position = a_pointA + distanceVector / 2f;
        a_spriteRenderer.transform.eulerAngles = new Vector3(0f, 0f, VLib.Vector3ToEulerAngle(distanceVector));
        a_spriteRenderer.transform.localScale = new Vector3(a_spriteRenderer.transform.localScale.x, desiredHeight,a_spriteRenderer.transform.localScale.z);
    }

    static internal void DrawTilingSpriteBetween2Points(SpriteRenderer a_spriteRenderer, Vector3 a_pointA, Vector3 a_pointB)
    {
        float textureHeight = a_spriteRenderer.sprite.texture.height;
        float pixelsPerUnit = a_spriteRenderer.sprite.pixelsPerUnit;

        Vector3 distanceVector = a_pointB - a_pointA;
        float distance = distanceVector.magnitude;
        float desiredHeight = distanceVector.magnitude / a_spriteRenderer.transform.localScale.y;
        float currentHeight = a_spriteRenderer.bounds.size.y;
        float scale = desiredHeight / currentHeight;

        a_spriteRenderer.transform.position = a_pointA + distanceVector / 2f;
        a_spriteRenderer.transform.eulerAngles = new Vector3(0f, 0f, VLib.Vector3ToEulerAngle(distanceVector));
        a_spriteRenderer.size = new Vector2(a_spriteRenderer.size.x, desiredHeight);
    }

    static internal void AddTriangle(List<int> a_triList, int a_firstVert, int a_secondVert, int a_thirdVert)
    {
        a_triList.Add(a_firstVert);
        a_triList.Add(a_secondVert);
        a_triList.Add(a_thirdVert);
    }

    static internal void AddQuad(List<int> a_triList, int a_firstVert, int a_secondVert, int a_thirdVert, int a_fourthVert)
    {
        AddTriangle(a_triList, a_firstVert, a_secondVert, a_thirdVert);
        AddTriangle(a_triList, a_firstVert, a_thirdVert, a_fourthVert);
    }
    static internal void Create3DMesh(GameObject a_gameObject, Material a_material, List <Vector3> a_verts, List<int> a_triangles)
    {
        Vector2[] uvs = new Vector2[a_verts.Count];
        for (int i = 0; i < a_verts.Count; i++)
        {
            uvs[i] = new Vector2(a_verts[i].x, a_verts[i].z) / 10f;
        }

        MeshRenderer meshRenderer = a_gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();

        mesh.vertices = a_verts.ToArray();
        mesh.triangles = a_triangles.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.SetUVs(0, uvs);

        MeshFilter meshFilter = a_gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        meshRenderer.material = a_material;
        a_gameObject.AddComponent<MeshCollider>().convex = true;
    }

    internal static void GetNiceDivisions(float a_maxValue, out int a_divisions, out float a_step)
    {
        a_divisions = 4;    // fallback
        a_step = a_maxValue / a_divisions;

        float bestScore = float.MaxValue;
        int[] preferredDivCounts = { 10, 9, 8, 7, 6, 5,4,3,2,1 };
        foreach (int div in preferredDivCounts)
        {
            float raw = a_maxValue / div;
            float nice = GetNiceNumber(raw);

            // Score: how close the nice number is to the raw segment size
            float score = Mathf.Abs(nice - raw);

            if (score < bestScore)
            {
                bestScore = score;
                a_divisions = div;
                a_step = nice;
            }
        }
    }

    // Rounds a number (like 233) to a "nice" number (200, 250, 300)
    private static float GetNiceNumber(float a_value)
    {
        float magnitude = Mathf.Pow(10, Mathf.Floor(Mathf.Log10(a_value)));
        float scaled = a_value / magnitude;

        float best = magnitude;

        foreach (float ns in new float[] { 1f, 2f, 3f, 4f, 5f, 10f })
        {
            float candidate = ns * magnitude;
            if (Mathf.Abs(candidate - a_value) < Mathf.Abs(best - a_value))
            {
                best = candidate;
            }
        }

        return best;
    }

    static internal float SinBetween(float a_smaller, float a_larger, float a_speed, float a_offset)
    {
        float diff = a_larger - a_smaller;
        float t = Time.time * a_speed + a_offset;
        return a_smaller + diff/2f + Mathf.Sin(t) * diff / 2f;
    }
}