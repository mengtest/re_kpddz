//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: red_pack.proto
namespace network
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"cs_red_pack_query_list_req")]
  public partial class cs_red_pack_query_list_req : global::ProtoBuf.IExtensible
  {
    public cs_red_pack_query_list_req() {}
    
    private uint _begin_id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"begin_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint begin_id
    {
      get { return _begin_id; }
      set { _begin_id = value; }
    }
    private uint _end_id;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"end_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint end_id
    {
      get { return _end_id; }
      set { _end_id = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"sc_red_pack_query_list_reply")]
  public partial class sc_red_pack_query_list_reply : global::ProtoBuf.IExtensible
  {
    public sc_red_pack_query_list_reply() {}
    
    private uint _begin_id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"begin_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint begin_id
    {
      get { return _begin_id; }
      set { _begin_id = value; }
    }
    private uint _end_id;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"end_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint end_id
    {
      get { return _end_id; }
      set { _end_id = value; }
    }
    private uint _max_num;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"max_num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint max_num
    {
      get { return _max_num; }
      set { _max_num = value; }
    }
    private readonly global::System.Collections.Generic.List<network.pb_red_pack_info> _list = new global::System.Collections.Generic.List<network.pb_red_pack_info>();
    [global::ProtoBuf.ProtoMember(4, Name=@"list", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<network.pb_red_pack_info> list
    {
      get { return _list; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"pb_red_pack_info")]
  public partial class pb_red_pack_info : global::ProtoBuf.IExtensible
  {
    public pb_red_pack_info() {}
    
    private string _uid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"uid", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string uid
    {
      get { return _uid; }
      set { _uid = value; }
    }
    private byte[] _player_name;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"player_name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public byte[] player_name
    {
      get { return _player_name; }
      set { _player_name = value; }
    }
    private byte[] _player_icon;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"player_icon", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public byte[] player_icon
    {
      get { return _player_icon; }
      set { _player_icon = value; }
    }
    private string _player_id;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"player_id", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string player_id
    {
      get { return _player_id; }
      set { _player_id = value; }
    }
    private ulong _min_num;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"min_num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong min_num
    {
      get { return _min_num; }
      set { _min_num = value; }
    }
    private ulong _max_num;
    [global::ProtoBuf.ProtoMember(6, IsRequired = true, Name=@"max_num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong max_num
    {
      get { return _max_num; }
      set { _max_num = value; }
    }
    private uint _over_time;
    [global::ProtoBuf.ProtoMember(7, IsRequired = true, Name=@"over_time", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint over_time
    {
      get { return _over_time; }
      set { _over_time = value; }
    }
    private byte[] _des;
    [global::ProtoBuf.ProtoMember(8, IsRequired = true, Name=@"des", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public byte[] des
    {
      get { return _des; }
      set { _des = value; }
    }
    private string _account;
    [global::ProtoBuf.ProtoMember(9, IsRequired = true, Name=@"account", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string account
    {
      get { return _account; }
      set { _account = value; }
    }
    private uint _sex;
    [global::ProtoBuf.ProtoMember(10, IsRequired = true, Name=@"sex", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint sex
    {
      get { return _sex; }
      set { _sex = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"cs_red_pack_open_req")]
  public partial class cs_red_pack_open_req : global::ProtoBuf.IExtensible
  {
    public cs_red_pack_open_req() {}
    
    private string _uid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"uid", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string uid
    {
      get { return _uid; }
      set { _uid = value; }
    }
    private ulong _check_num;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"check_num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong check_num
    {
      get { return _check_num; }
      set { _check_num = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"sc_red_pack_open_reply")]
  public partial class sc_red_pack_open_reply : global::ProtoBuf.IExtensible
  {
    public sc_red_pack_open_reply() {}
    
    private uint _result;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint result
    {
      get { return _result; }
      set { _result = value; }
    }
    private byte[] _err;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"err", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public byte[] err
    {
      get { return _err; }
      set { _err = value; }
    }
    private ulong _reward_num;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"reward_num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong reward_num
    {
      get { return _reward_num; }
      set { _reward_num = value; }
    }
    private string _uid;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"uid", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string uid
    {
      get { return _uid; }
      set { _uid = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"cs_red_pack_create_req")]
  public partial class cs_red_pack_create_req : global::ProtoBuf.IExtensible
  {
    public cs_red_pack_create_req() {}
    
    private ulong _set_num;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"set_num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong set_num
    {
      get { return _set_num; }
      set { _set_num = value; }
    }
    private byte[] _des;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"des", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public byte[] des
    {
      get { return _des; }
      set { _des = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"sc_red_pack_create_reply")]
  public partial class sc_red_pack_create_reply : global::ProtoBuf.IExtensible
  {
    public sc_red_pack_create_reply() {}
    
    private uint _result;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint result
    {
      get { return _result; }
      set { _result = value; }
    }
    private byte[] _err;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"err", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public byte[] err
    {
      get { return _err; }
      set { _err = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"sc_red_pack_notice_update")]
  public partial class sc_red_pack_notice_update : global::ProtoBuf.IExtensible
  {
    public sc_red_pack_notice_update() {}
    
    private readonly global::System.Collections.Generic.List<network.pb_red_pack_notice> _list = new global::System.Collections.Generic.List<network.pb_red_pack_notice>();
    [global::ProtoBuf.ProtoMember(1, Name=@"list", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<network.pb_red_pack_notice> list
    {
      get { return _list; }
    }
  
    private readonly global::System.Collections.Generic.List<string> _delete_notice_list = new global::System.Collections.Generic.List<string>();
    [global::ProtoBuf.ProtoMember(2, Name=@"delete_notice_list", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<string> delete_notice_list
    {
      get { return _delete_notice_list; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"pb_red_pack_notice")]
  public partial class pb_red_pack_notice : global::ProtoBuf.IExtensible
  {
    public pb_red_pack_notice() {}
    
    private string _notice_id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"notice_id", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string notice_id
    {
      get { return _notice_id; }
      set { _notice_id = value; }
    }
    private uint _notice_type;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"notice_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint notice_type
    {
      get { return _notice_type; }
      set { _notice_type = value; }
    }
    private uint _get_sec_time;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"get_sec_time", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint get_sec_time
    {
      get { return _get_sec_time; }
      set { _get_sec_time = value; }
    }
    private byte[] _content;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"content", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public byte[] content
    {
      get { return _content; }
      set { _content = value; }
    }
    private ulong _gold_num;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"gold_num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong gold_num
    {
      get { return _gold_num; }
      set { _gold_num = value; }
    }
    private uint _type;
    [global::ProtoBuf.ProtoMember(6, IsRequired = true, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint type
    {
      get { return _type; }
      set { _type = value; }
    }
    private byte[] _open_player_name;
    [global::ProtoBuf.ProtoMember(7, IsRequired = true, Name=@"open_player_name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public byte[] open_player_name
    {
      get { return _open_player_name; }
      set { _open_player_name = value; }
    }
    private string _open_player_account;
    [global::ProtoBuf.ProtoMember(8, IsRequired = true, Name=@"open_player_account", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string open_player_account
    {
      get { return _open_player_account; }
      set { _open_player_account = value; }
    }
    private string _uid;
    [global::ProtoBuf.ProtoMember(9, IsRequired = true, Name=@"uid", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string uid
    {
      get { return _uid; }
      set { _uid = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"cs_red_pack_cancel_req")]
  public partial class cs_red_pack_cancel_req : global::ProtoBuf.IExtensible
  {
    public cs_red_pack_cancel_req() {}
    
    private string _uid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"uid", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string uid
    {
      get { return _uid; }
      set { _uid = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"sc_red_pack_cancel_reply")]
  public partial class sc_red_pack_cancel_reply : global::ProtoBuf.IExtensible
  {
    public sc_red_pack_cancel_reply() {}
    
    private uint _result;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint result
    {
      get { return _result; }
      set { _result = value; }
    }
    private byte[] _err;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"err", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public byte[] err
    {
      get { return _err; }
      set { _err = value; }
    }
    private string _uid;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"uid", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string uid
    {
      get { return _uid; }
      set { _uid = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"sc_self_red_pack_info")]
  public partial class sc_self_red_pack_info : global::ProtoBuf.IExtensible
  {
    public sc_self_red_pack_info() {}
    
    private uint _all_red_pack_num;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"all_red_pack_num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint all_red_pack_num
    {
      get { return _all_red_pack_num; }
      set { _all_red_pack_num = value; }
    }
    private readonly global::System.Collections.Generic.List<network.pb_red_pack_info> _red_pack_list = new global::System.Collections.Generic.List<network.pb_red_pack_info>();
    [global::ProtoBuf.ProtoMember(2, Name=@"red_pack_list", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<network.pb_red_pack_info> red_pack_list
    {
      get { return _red_pack_list; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"cs_red_pack_do_select_req")]
  public partial class cs_red_pack_do_select_req : global::ProtoBuf.IExtensible
  {
    public cs_red_pack_do_select_req() {}
    
    private string _notice_id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"notice_id", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string notice_id
    {
      get { return _notice_id; }
      set { _notice_id = value; }
    }
    private uint _opt;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"opt", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint opt
    {
      get { return _opt; }
      set { _opt = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"sc_red_pack_do_select_reply")]
  public partial class sc_red_pack_do_select_reply : global::ProtoBuf.IExtensible
  {
    public sc_red_pack_do_select_reply() {}
    
    private uint _result;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint result
    {
      get { return _result; }
      set { _result = value; }
    }

    private byte[] _err_msg = null;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"err_msg", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public byte[] err_msg
    {
      get { return _err_msg; }
      set { _err_msg = value; }
    }
    private string _redpack_id;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"redpack_id", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string redpack_id
    {
      get { return _redpack_id; }
      set { _redpack_id = value; }
    }
    private uint _opt;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"opt", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint opt
    {
      get { return _opt; }
      set { _opt = value; }
    }
    private string _notice_id;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"notice_id", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string notice_id
    {
      get { return _notice_id; }
      set { _notice_id = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"cs_red_pack_search_req")]
  public partial class cs_red_pack_search_req : global::ProtoBuf.IExtensible
  {
    public cs_red_pack_search_req() {}
    
    private string _uid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"uid", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string uid
    {
      get { return _uid; }
      set { _uid = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"sc_red_pack_search_reply")]
  public partial class sc_red_pack_search_reply : global::ProtoBuf.IExtensible
  {
    public sc_red_pack_search_reply() {}
    
    private readonly global::System.Collections.Generic.List<network.pb_red_pack_info> _list = new global::System.Collections.Generic.List<network.pb_red_pack_info>();
    [global::ProtoBuf.ProtoMember(1, Name=@"list", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<network.pb_red_pack_info> list
    {
      get { return _list; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}