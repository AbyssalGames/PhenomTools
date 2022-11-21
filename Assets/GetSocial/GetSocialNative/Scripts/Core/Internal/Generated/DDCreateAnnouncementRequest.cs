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

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class DDCreateAnnouncementRequest : TBase
  {
    private string _sessionId;
    private string _appId;
    private Dictionary<string, AFContent> _content;
    private long _startDate;
    private long _endDate;
    private Dictionary<string, string> _properties;
    private SGEntity _target;
    private AFPollContent _poll;
    private bool _allowMultiReactions;
    private List<string> _labels;

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

    public Dictionary<string, AFContent> Content
    {
      get
      {
        return _content;
      }
      set
      {
        __isset.content = true;
        this._content = value;
      }
    }

    public long StartDate
    {
      get
      {
        return _startDate;
      }
      set
      {
        __isset.startDate = true;
        this._startDate = value;
      }
    }

    public long EndDate
    {
      get
      {
        return _endDate;
      }
      set
      {
        __isset.endDate = true;
        this._endDate = value;
      }
    }

    public Dictionary<string, string> Properties
    {
      get
      {
        return _properties;
      }
      set
      {
        __isset.properties = true;
        this._properties = value;
      }
    }

    public SGEntity Target
    {
      get
      {
        return _target;
      }
      set
      {
        __isset.target = true;
        this._target = value;
      }
    }

    public AFPollContent Poll
    {
      get
      {
        return _poll;
      }
      set
      {
        __isset.poll = true;
        this._poll = value;
      }
    }

    public bool AllowMultiReactions
    {
      get
      {
        return _allowMultiReactions;
      }
      set
      {
        __isset.allowMultiReactions = true;
        this._allowMultiReactions = value;
      }
    }

    public List<string> Labels
    {
      get
      {
        return _labels;
      }
      set
      {
        __isset.labels = true;
        this._labels = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool sessionId;
      public bool appId;
      public bool content;
      public bool startDate;
      public bool endDate;
      public bool properties;
      public bool target;
      public bool poll;
      public bool allowMultiReactions;
      public bool labels;
    }

    public DDCreateAnnouncementRequest() {
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
              if (field.Type == TType.Map) {
                {
                  Content = new Dictionary<string, AFContent>();
                  TMap _map273 = iprot.ReadMapBegin();
                  for( int _i274 = 0; _i274 < _map273.Count; ++_i274)
                  {
                    string _key275;
                    AFContent _val276;
                    _key275 = iprot.ReadString();
                    _val276 = new AFContent();
                    _val276.Read(iprot);
                    Content[_key275] = _val276;
                  }
                  iprot.ReadMapEnd();
                }
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.I64) {
                StartDate = iprot.ReadI64();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 5:
              if (field.Type == TType.I64) {
                EndDate = iprot.ReadI64();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 6:
              if (field.Type == TType.Map) {
                {
                  Properties = new Dictionary<string, string>();
                  TMap _map277 = iprot.ReadMapBegin();
                  for( int _i278 = 0; _i278 < _map277.Count; ++_i278)
                  {
                    string _key279;
                    string _val280;
                    _key279 = iprot.ReadString();
                    _val280 = iprot.ReadString();
                    Properties[_key279] = _val280;
                  }
                  iprot.ReadMapEnd();
                }
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 7:
              if (field.Type == TType.Struct) {
                Target = new SGEntity();
                Target.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 8:
              if (field.Type == TType.Struct) {
                Poll = new AFPollContent();
                Poll.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 9:
              if (field.Type == TType.Bool) {
                AllowMultiReactions = iprot.ReadBool();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 10:
              if (field.Type == TType.List) {
                {
                  Labels = new List<string>();
                  TList _list281 = iprot.ReadListBegin();
                  for( int _i282 = 0; _i282 < _list281.Count; ++_i282)
                  {
                    string _elem283;
                    _elem283 = iprot.ReadString();
                    Labels.Add(_elem283);
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
        TStruct struc = new TStruct("DDCreateAnnouncementRequest");
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
        if (Content != null && __isset.content) {
          field.Name = "content";
          field.Type = TType.Map;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteMapBegin(new TMap(TType.String, TType.Struct, Content.Count));
            foreach (string _iter284 in Content.Keys)
            {
              oprot.WriteString(_iter284);
              Content[_iter284].Write(oprot);
            }
            oprot.WriteMapEnd();
          }
          oprot.WriteFieldEnd();
        }
        if (__isset.startDate) {
          field.Name = "startDate";
          field.Type = TType.I64;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          oprot.WriteI64(StartDate);
          oprot.WriteFieldEnd();
        }
        if (__isset.endDate) {
          field.Name = "endDate";
          field.Type = TType.I64;
          field.ID = 5;
          oprot.WriteFieldBegin(field);
          oprot.WriteI64(EndDate);
          oprot.WriteFieldEnd();
        }
        if (Properties != null && __isset.properties) {
          field.Name = "properties";
          field.Type = TType.Map;
          field.ID = 6;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteMapBegin(new TMap(TType.String, TType.String, Properties.Count));
            foreach (string _iter285 in Properties.Keys)
            {
              oprot.WriteString(_iter285);
              oprot.WriteString(Properties[_iter285]);
            }
            oprot.WriteMapEnd();
          }
          oprot.WriteFieldEnd();
        }
        if (Target != null && __isset.target) {
          field.Name = "target";
          field.Type = TType.Struct;
          field.ID = 7;
          oprot.WriteFieldBegin(field);
          Target.Write(oprot);
          oprot.WriteFieldEnd();
        }
        if (Poll != null && __isset.poll) {
          field.Name = "poll";
          field.Type = TType.Struct;
          field.ID = 8;
          oprot.WriteFieldBegin(field);
          Poll.Write(oprot);
          oprot.WriteFieldEnd();
        }
        if (__isset.allowMultiReactions) {
          field.Name = "allowMultiReactions";
          field.Type = TType.Bool;
          field.ID = 9;
          oprot.WriteFieldBegin(field);
          oprot.WriteBool(AllowMultiReactions);
          oprot.WriteFieldEnd();
        }
        if (Labels != null && __isset.labels) {
          field.Name = "labels";
          field.Type = TType.List;
          field.ID = 10;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteListBegin(new TList(TType.String, Labels.Count));
            foreach (string _iter286 in Labels)
            {
              oprot.WriteString(_iter286);
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
      StringBuilder __sb = new StringBuilder("DDCreateAnnouncementRequest(");
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
      if (Content != null && __isset.content) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Content: ");
        __sb.Append(Content.ToDebugString());
      }
      if (__isset.startDate) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("StartDate: ");
        __sb.Append(StartDate);
      }
      if (__isset.endDate) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("EndDate: ");
        __sb.Append(EndDate);
      }
      if (Properties != null && __isset.properties) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Properties: ");
        __sb.Append(Properties.ToDebugString());
      }
      if (Target != null && __isset.target) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Target: ");
        __sb.Append(Target== null ? "<null>" : Target.ToString());
      }
      if (Poll != null && __isset.poll) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Poll: ");
        __sb.Append(Poll== null ? "<null>" : Poll.ToString());
      }
      if (__isset.allowMultiReactions) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("AllowMultiReactions: ");
        __sb.Append(AllowMultiReactions);
      }
      if (Labels != null && __isset.labels) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Labels: ");
        __sb.Append(Labels.ToDebugString());
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
