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
  public partial class TestDeviceRemoteLog : TBase
  {
    private string _rpc;
    private string _request;
    private string _session;
    private string _responseTitle;
    private string _response;
    private long _createdAt;

    public string Rpc
    {
      get
      {
        return _rpc;
      }
      set
      {
        __isset.rpc = true;
        this._rpc = value;
      }
    }

    public string Request
    {
      get
      {
        return _request;
      }
      set
      {
        __isset.request = true;
        this._request = value;
      }
    }

    public string Session
    {
      get
      {
        return _session;
      }
      set
      {
        __isset.session = true;
        this._session = value;
      }
    }

    public string ResponseTitle
    {
      get
      {
        return _responseTitle;
      }
      set
      {
        __isset.responseTitle = true;
        this._responseTitle = value;
      }
    }

    public string Response
    {
      get
      {
        return _response;
      }
      set
      {
        __isset.response = true;
        this._response = value;
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


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool rpc;
      public bool request;
      public bool session;
      public bool responseTitle;
      public bool response;
      public bool createdAt;
    }

    public TestDeviceRemoteLog() {
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
                Rpc = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.String) {
                Request = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.String) {
                Session = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.String) {
                ResponseTitle = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 5:
              if (field.Type == TType.String) {
                Response = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 6:
              if (field.Type == TType.I64) {
                CreatedAt = iprot.ReadI64();
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
        TStruct struc = new TStruct("TestDeviceRemoteLog");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (Rpc != null && __isset.rpc) {
          field.Name = "rpc";
          field.Type = TType.String;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Rpc);
          oprot.WriteFieldEnd();
        }
        if (Request != null && __isset.request) {
          field.Name = "request";
          field.Type = TType.String;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Request);
          oprot.WriteFieldEnd();
        }
        if (Session != null && __isset.session) {
          field.Name = "session";
          field.Type = TType.String;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Session);
          oprot.WriteFieldEnd();
        }
        if (ResponseTitle != null && __isset.responseTitle) {
          field.Name = "responseTitle";
          field.Type = TType.String;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(ResponseTitle);
          oprot.WriteFieldEnd();
        }
        if (Response != null && __isset.response) {
          field.Name = "response";
          field.Type = TType.String;
          field.ID = 5;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Response);
          oprot.WriteFieldEnd();
        }
        if (__isset.createdAt) {
          field.Name = "createdAt";
          field.Type = TType.I64;
          field.ID = 6;
          oprot.WriteFieldBegin(field);
          oprot.WriteI64(CreatedAt);
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
      StringBuilder __sb = new StringBuilder("TestDeviceRemoteLog(");
      bool __first = true;
      if (Rpc != null && __isset.rpc) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Rpc: ");
        __sb.Append(Rpc);
      }
      if (Request != null && __isset.request) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Request: ");
        __sb.Append(Request);
      }
      if (Session != null && __isset.session) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Session: ");
        __sb.Append(Session);
      }
      if (ResponseTitle != null && __isset.responseTitle) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("ResponseTitle: ");
        __sb.Append(ResponseTitle);
      }
      if (Response != null && __isset.response) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Response: ");
        __sb.Append(Response);
      }
      if (__isset.createdAt) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("CreatedAt: ");
        __sb.Append(CreatedAt);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
