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
  /// #sdk7
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class SGChat : TBase
  {
    private string _id;
    private string _title;
    private string _avatarUrl;
    private long _createdAt;
    private long _updatedAt;
    private string _createdBy;
    private int _membersCount;
    private SGSettings _settings;
    private SGChatMessage _lastMessage;
    private SGChatType _type;
    private THCreator _otherMember;

    public string Id
    {
      get
      {
        return _id;
      }
      set
      {
        __isset.id = true;
        this._id = value;
      }
    }

    public string Title
    {
      get
      {
        return _title;
      }
      set
      {
        __isset.title = true;
        this._title = value;
      }
    }

    public string AvatarUrl
    {
      get
      {
        return _avatarUrl;
      }
      set
      {
        __isset.avatarUrl = true;
        this._avatarUrl = value;
      }
    }

    public long CreatedAt
    {
      get
      {
        return _createdAt;
      }
      set
      {
        __isset.createdAt = true;
        this._createdAt = value;
      }
    }

    public long UpdatedAt
    {
      get
      {
        return _updatedAt;
      }
      set
      {
        __isset.updatedAt = true;
        this._updatedAt = value;
      }
    }

    /// <summary>
    /// user id of creator
    /// </summary>
    public string CreatedBy
    {
      get
      {
        return _createdBy;
      }
      set
      {
        __isset.createdBy = true;
        this._createdBy = value;
      }
    }

    public int MembersCount
    {
      get
      {
        return _membersCount;
      }
      set
      {
        __isset.membersCount = true;
        this._membersCount = value;
      }
    }

    public SGSettings Settings
    {
      get
      {
        return _settings;
      }
      set
      {
        __isset.settings = true;
        this._settings = value;
      }
    }

    public SGChatMessage LastMessage
    {
      get
      {
        return _lastMessage;
      }
      set
      {
        __isset.lastMessage = true;
        this._lastMessage = value;
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="SGChatType"/>
    /// </summary>
    public SGChatType Type
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

    public THCreator OtherMember
    {
      get
      {
        return _otherMember;
      }
      set
      {
        __isset.otherMember = true;
        this._otherMember = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool id;
      public bool title;
      public bool avatarUrl;
      public bool createdAt;
      public bool updatedAt;
      public bool createdBy;
      public bool membersCount;
      public bool settings;
      public bool lastMessage;
      public bool type;
      public bool otherMember;
    }

    public SGChat() {
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
                Id = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.String) {
                Title = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.String) {
                AvatarUrl = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.I64) {
                CreatedAt = iprot.ReadI64();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 5:
              if (field.Type == TType.I64) {
                UpdatedAt = iprot.ReadI64();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 6:
              if (field.Type == TType.String) {
                CreatedBy = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 7:
              if (field.Type == TType.I32) {
                MembersCount = iprot.ReadI32();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 8:
              if (field.Type == TType.Struct) {
                Settings = new SGSettings();
                Settings.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 9:
              if (field.Type == TType.Struct) {
                LastMessage = new SGChatMessage();
                LastMessage.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 10:
              if (field.Type == TType.I32) {
                Type = (SGChatType)iprot.ReadI32();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 11:
              if (field.Type == TType.Struct) {
                OtherMember = new THCreator();
                OtherMember.Read(iprot);
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
        TStruct struc = new TStruct("SGChat");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (Id != null && __isset.id) {
          field.Name = "id";
          field.Type = TType.String;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Id);
          oprot.WriteFieldEnd();
        }
        if (Title != null && __isset.title) {
          field.Name = "title";
          field.Type = TType.String;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Title);
          oprot.WriteFieldEnd();
        }
        if (AvatarUrl != null && __isset.avatarUrl) {
          field.Name = "avatarUrl";
          field.Type = TType.String;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(AvatarUrl);
          oprot.WriteFieldEnd();
        }
        if (__isset.createdAt) {
          field.Name = "createdAt";
          field.Type = TType.I64;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          oprot.WriteI64(CreatedAt);
          oprot.WriteFieldEnd();
        }
        if (__isset.updatedAt) {
          field.Name = "updatedAt";
          field.Type = TType.I64;
          field.ID = 5;
          oprot.WriteFieldBegin(field);
          oprot.WriteI64(UpdatedAt);
          oprot.WriteFieldEnd();
        }
        if (CreatedBy != null && __isset.createdBy) {
          field.Name = "createdBy";
          field.Type = TType.String;
          field.ID = 6;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(CreatedBy);
          oprot.WriteFieldEnd();
        }
        if (__isset.membersCount) {
          field.Name = "membersCount";
          field.Type = TType.I32;
          field.ID = 7;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32(MembersCount);
          oprot.WriteFieldEnd();
        }
        if (Settings != null && __isset.settings) {
          field.Name = "settings";
          field.Type = TType.Struct;
          field.ID = 8;
          oprot.WriteFieldBegin(field);
          Settings.Write(oprot);
          oprot.WriteFieldEnd();
        }
        if (LastMessage != null && __isset.lastMessage) {
          field.Name = "lastMessage";
          field.Type = TType.Struct;
          field.ID = 9;
          oprot.WriteFieldBegin(field);
          LastMessage.Write(oprot);
          oprot.WriteFieldEnd();
        }
        if (__isset.type) {
          field.Name = "type";
          field.Type = TType.I32;
          field.ID = 10;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32((int)Type);
          oprot.WriteFieldEnd();
        }
        if (OtherMember != null && __isset.otherMember) {
          field.Name = "otherMember";
          field.Type = TType.Struct;
          field.ID = 11;
          oprot.WriteFieldBegin(field);
          OtherMember.Write(oprot);
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
      StringBuilder __sb = new StringBuilder("SGChat(");
      bool __first = true;
      if (Id != null && __isset.id) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Id: ");
        __sb.Append(Id);
      }
      if (Title != null && __isset.title) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Title: ");
        __sb.Append(Title);
      }
      if (AvatarUrl != null && __isset.avatarUrl) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("AvatarUrl: ");
        __sb.Append(AvatarUrl);
      }
      if (__isset.createdAt) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("CreatedAt: ");
        __sb.Append(CreatedAt);
      }
      if (__isset.updatedAt) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("UpdatedAt: ");
        __sb.Append(UpdatedAt);
      }
      if (CreatedBy != null && __isset.createdBy) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("CreatedBy: ");
        __sb.Append(CreatedBy);
      }
      if (__isset.membersCount) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("MembersCount: ");
        __sb.Append(MembersCount);
      }
      if (Settings != null && __isset.settings) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Settings: ");
        __sb.Append(Settings== null ? "<null>" : Settings.ToString());
      }
      if (LastMessage != null && __isset.lastMessage) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("LastMessage: ");
        __sb.Append(LastMessage);
      }
      if (__isset.type) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Type: ");
        __sb.Append(Type);
      }
      if (OtherMember != null && __isset.otherMember) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("OtherMember: ");
        __sb.Append(OtherMember== null ? "<null>" : OtherMember.ToString());
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
