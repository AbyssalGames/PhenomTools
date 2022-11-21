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
  /// remove members
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class DDRemoveGroupMembersRequest : TBase
  {
    private string _sessionId;
    private string _appId;
    private string _groupId;
    private List<string> _userIds;

    public string SessionId
    {
      get
      {
        return _sessionId;
      }
      set
      {
        __isset.sessionId = true;
        this._sessionId = value;
      }
    }

    public string AppId
    {
      get
      {
        return _appId;
      }
      set
      {
        __isset.appId = true;
        this._appId = value;
      }
    }

    public string GroupId
    {
      get
      {
        return _groupId;
      }
      set
      {
        __isset.groupId = true;
        this._groupId = value;
      }
    }

    public List<string> UserIds
    {
      get
      {
        return _userIds;
      }
      set
      {
        __isset.userIds = true;
        this._userIds = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool sessionId;
      public bool appId;
      public bool groupId;
      public bool userIds;
    }

    public DDRemoveGroupMembersRequest() {
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
                SessionId = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.String) {
                AppId = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.String) {
                GroupId = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.List) {
                {
                  UserIds = new List<string>();
                  TList _list177 = iprot.ReadListBegin();
                  for( int _i178 = 0; _i178 < _list177.Count; ++_i178)
                  {
                    string _elem179;
                    _elem179 = iprot.ReadString();
                    UserIds.Add(_elem179);
                  }
                  iprot.ReadListEnd();
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
        TStruct struc = new TStruct("DDRemoveGroupMembersRequest");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (SessionId != null && __isset.sessionId) {
          field.Name = "sessionId";
          field.Type = TType.String;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(SessionId);
          oprot.WriteFieldEnd();
        }
        if (AppId != null && __isset.appId) {
          field.Name = "appId";
          field.Type = TType.String;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(AppId);
          oprot.WriteFieldEnd();
        }
        if (GroupId != null && __isset.groupId) {
          field.Name = "groupId";
          field.Type = TType.String;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(GroupId);
          oprot.WriteFieldEnd();
        }
        if (UserIds != null && __isset.userIds) {
          field.Name = "userIds";
          field.Type = TType.List;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteListBegin(new TList(TType.String, UserIds.Count));
            foreach (string _iter180 in UserIds)
            {
              oprot.WriteString(_iter180);
            }
            oprot.WriteListEnd();
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
      StringBuilder __sb = new StringBuilder("DDRemoveGroupMembersRequest(");
      bool __first = true;
      if (SessionId != null && __isset.sessionId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("SessionId: ");
        __sb.Append(SessionId);
      }
      if (AppId != null && __isset.appId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("AppId: ");
        __sb.Append(AppId);
      }
      if (GroupId != null && __isset.groupId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("GroupId: ");
        __sb.Append(GroupId);
      }
      if (UserIds != null && __isset.userIds) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("UserIds: ");
        __sb.Append(UserIds.ToDebugString());
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
