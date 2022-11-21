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
  /// #todo_sdk7
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class THInviteChannelV2 : TBase
  {
    private string _name;
    private string _providerId;
    private int _displayOrder;
    private string _iconUrl;

    public string Name
    {
      get
      {
        return _name;
      }
      set
      {
        __isset.name = true;
        this._name = value;
      }
    }

    public string ProviderId
    {
      get
      {
        return _providerId;
      }
      set
      {
        __isset.providerId = true;
        this._providerId = value;
      }
    }

    public int DisplayOrder
    {
      get
      {
        return _displayOrder;
      }
      set
      {
        __isset.displayOrder = true;
        this._displayOrder = value;
      }
    }

    public string IconUrl
    {
      get
      {
        return _iconUrl;
      }
      set
      {
        __isset.iconUrl = true;
        this._iconUrl = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool name;
      public bool providerId;
      public bool displayOrder;
      public bool iconUrl;
    }

    public THInviteChannelV2() {
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
                Name = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.String) {
                ProviderId = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.I32) {
                DisplayOrder = iprot.ReadI32();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.String) {
                IconUrl = iprot.ReadString();
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
        TStruct struc = new TStruct("THInviteChannelV2");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (Name != null && __isset.name) {
          field.Name = "name";
          field.Type = TType.String;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Name);
          oprot.WriteFieldEnd();
        }
        if (ProviderId != null && __isset.providerId) {
          field.Name = "providerId";
          field.Type = TType.String;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(ProviderId);
          oprot.WriteFieldEnd();
        }
        if (__isset.displayOrder) {
          field.Name = "displayOrder";
          field.Type = TType.I32;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32(DisplayOrder);
          oprot.WriteFieldEnd();
        }
        if (IconUrl != null && __isset.iconUrl) {
          field.Name = "iconUrl";
          field.Type = TType.String;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(IconUrl);
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
      StringBuilder __sb = new StringBuilder("THInviteChannelV2(");
      bool __first = true;
      if (Name != null && __isset.name) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Name: ");
        __sb.Append(Name);
      }
      if (ProviderId != null && __isset.providerId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("ProviderId: ");
        __sb.Append(ProviderId);
      }
      if (__isset.displayOrder) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("DisplayOrder: ");
        __sb.Append(DisplayOrder);
      }
      if (IconUrl != null && __isset.iconUrl) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("IconUrl: ");
        __sb.Append(IconUrl);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
