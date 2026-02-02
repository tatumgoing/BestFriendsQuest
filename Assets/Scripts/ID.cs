using UnityEngine;

[System.Serializable]
public class ID : System.IEquatable<ID>, System.IEquatable<string>, System.IEquatable<int>
{
    public string Value;

    public override int GetHashCode() => Value.GetHashCode();
    public ID(int value) => Value = FromInt(value); 
    public ID(string value) => Value = value;
    public override string ToString() => Value;

    public ID()
    {
        Value = "0000";
    }

    public void GenerateNew()
    {
        Value = "";
        for (int i = 0; i < SaveSystem.IDLength; i++) {
            Value += Random.Range(1, 10).ToString();
        }
    }

    public static string FromInt(int value)
    {
        var resultString = value.ToString();
        while (resultString.Length < 4) resultString = "0" + resultString;
        return resultString;
    }

    public override bool Equals(object other)
    {
        if (other is ID IDID) return string.Equals(Value, IDID.Value);
        if (other is string stringID) return string.Equals(Value, stringID);
        if (other is int intID) return string.Equals(Value, FromInt(intID));
        return false;
    }

    public static bool operator ==(ID a, string b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
        return string.Equals(a.Value, b);
    }

    public static bool operator ==(ID a, ID b) => a == b.Value;
    public static bool operator !=(ID a, ID b) => !(a == b);
    public static bool operator !=(ID a, string b) => !(a == b);
    public static bool operator ==(ID a, int b) => a == FromInt(b);
    public static bool operator !=(ID a, int b) => !(a == b);

    public static implicit operator string(ID id) => id.Value;
    public static implicit operator ID(string str) => new ID(str);
    public static implicit operator int(ID id) => int.Parse(id.Value);

    bool System.IEquatable<ID>.Equals(ID other)
    {
        if (ReferenceEquals(other, null)) return false;
        return string.Equals(Value, other.Value);
    }
    bool System.IEquatable<string>.Equals(string other) => string.Equals(Value, other);
    bool System.IEquatable<int>.Equals(int other) => string.Equals(Value, FromInt(other));
}
