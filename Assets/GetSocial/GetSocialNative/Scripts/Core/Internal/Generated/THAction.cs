#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
/**
 * Autogenerated by Thrift Compiler ()
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;

namespace GetSocialSdk.Core 
{

  /// <summary>
  /// #sdk6 #sdk7
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class THAction : TBase
  {
    private string _type;
    private Dictionary<string, string> _data;

    public string Type
    {
      get
      {
        return _type;
      }
      set
      {
        __isset.type = true;
        this._type = value;
      }
    }

    public Dictionary<string, string> Data
    {
      get
      {
        return _data;
      }
      set
      {
        __isset.data = true;
        this._data = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool type;
      public bool data;
    }

    public THAction() {
    }

    public void Read (TProtocol iprot)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        TField field;
        iprot.ReadStructBegin();
        while (true)
        {
          field = iprot.ReadFieldBegin();
          if (field.Type == TType.Stop) { 
            break;
          }
          switch (field.ID)
          {
            case 1:
              if (field.Type == TType.String) {
                Type = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.Map) {
                {
                  Data = new Dictionary<string, string>();
                  TMap _map0 = iprot.ReadMapBegin();
                  for( int _i1 = 0; _i1 < _map0.Count; ++_i1)
                  {
                    string _key2;
                    string _val3;
                    _key2 = iprot.ReadString();
                    _val3 = iprot.ReadString();
                    Data[_key2] = _val3;
                  }
                  iprot.ReadMapEnd();
                }
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            default: 
              TProtocolUtil.Skip(iprot, field.Type);
              break;
          }
          iprot.ReadFieldEnd();
        }
        iprot.ReadStructEnd();
      }
      finally
      {
        iprot.DecrementRecursionDepth();
      }
    }

    public void Write(TProtocol oprot) {
      oprot.IncrementRecursionDepth();
      try
      {
        TStruct struc = new TStruct("THAction");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (Type != null && __isset.type) {
          field.Name = "type";
          field.Type = TType.String;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Type);
          oprot.WriteFieldEnd();
        }
        if (Data != null && __isset.data) {
          field.Name = "data";
          field.Type = TType.Map;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteMapBegin(new TMap(TType.String, TType.String, Data.Count));
            foreach (string _iter4 in Data.Keys)
            {
              oprot.WriteString(_iter4);
              oprot.WriteString(Data[_iter4]);
            }
            oprot.WriteMapEnd();
          }
          oprot.WriteFieldEnd();
        }
        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }
      finally
      {
        oprot.DecrementRecursionDepth();
      }
    }

    public override string ToString() {
      StringBuilder __sb = new StringBuilder("THAction(");
      bool __first = true;
      if (Type != null && __isset.type) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Type: ");
        __sb.Append(Type);
      }
      if (Data != null && __isset.data) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Data: ");
        __sb.Append(Data.ToDebugString());
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
