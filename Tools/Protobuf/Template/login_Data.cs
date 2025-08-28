using GameServer.MySQL;
using Hukiry.Protobuf;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GameServer.TcpServer
{
    public class login_Data
    {
        [TextLength(ProtoType.PrimaryKey)] public int roleID =0;//角色id （开发：1到10000；发布，10001以上）
        [TextLength(ProtoType.Text,100)] public string deviceID;//设备唯一标示符
        [TextLength(ProtoType.FullText)] public string token;//sdk或者密码
        [TextLength(ProtoType.None)] public int platform;//系统平台
        [TextLength(ProtoType.None)] public int loginType;//登陆类型
        [TextLength(ProtoType.Text, 10)] public string lanCode;//语言代码
        [TextLength(ProtoType.None)] public int createTime = 0;//注册时间
        [TextLength(ProtoType.None)] public short portraitID = 0;
        /// <summary>
        /// 角色昵称
        /// </summary>
        [TextLength(ProtoType.Text, 20)] public string roleNick = string.Empty;
        /// <summary>
        /// ip地址
        /// </summary>
        [TextLength(ProtoType.Text|ProtoType.Custome, 30)] public string ipAddress;

        //TEXT INT BIT VARCHAR(50)
        public void ReadSQL(string clientID)
        {
            this.CreateTable();
            SQLTable.DealSqlData(this, SQLTable.RoleTable);
            this.UpdateSqlData();

            string sqlSelect = string.IsNullOrEmpty(this.token) ? $"{nameof(deviceID)}='{this.deviceID}'" : $"{nameof(token)}='{this.token}'";
            GameMysql.instance.SelectTable(SQLTable.RoleTable, sqlSelect, (data, isOk) => {
                if (isOk)
                {
                    this.roleID = data.GetInt32(nameof(roleID));
                    this.deviceID = data.GetString(nameof(deviceID));
                    this.token = data.GetString(nameof(token));
                    this.platform = data.GetInt32(nameof(platform));
                    this.loginType = data.GetInt32(nameof(loginType));
                    this.lanCode = data.GetString(nameof(lanCode));
                    this.createTime = data.GetInt32(nameof(createTime));
                    this.portraitID = (short)data.GetInt32(nameof(portraitID));
                    this.roleNick = data.GetString(nameof(roleNick));
                    this.ipAddress = data.GetString(nameof(ipAddress));

                    //处理上一次数据的逻辑
                }
                else
                {
                    this.CreateRole();
                }
            });
            //设置用户id
            GameCenter.instance.GetGameTcpServer().GetClientData(clientID)?.SetRoleID(this.roleID);
        }

        public void CreateTable()
        {
            if (GameMysql.instance.IsTableExists(SQLTable.RoleTable)) return;
            SQLTable.DealSqlData(this, item => {
                GameMysql.instance.CreateTable(SQLTable.RoleTable, item.creatSql);
            });
        }

        public void UpdateSqlData()
        {
            SQLTable.DealSqlData(this, item => {
                GameMysql.instance.UpdateTable(SQLTable.RoleTable, item.updateSql, item.mainKeyName, item.mainValue);
            });
        }

        /// <summary>
        /// 创建角色，保存新数据
        /// </summary>
        private void CreateRole()
        {
            GlobalData.SumUserCount++;
            this.createTime = (int)GameCenter.GetTimeSecond();
            this.roleID = (ServerConifg.isDevelop ? 0 : 10000) + GlobalData.SumUserCount;
            SQLTable.DealSqlData(this, item => {
                GameMysql.instance.InsertTable(SQLTable.RoleTable, item.fieldKeys, item.fieldKeys);
            });
        }

        //创建资源物品
        public List<GMoneyInfo> ReadItemSQL()
        {
            List<GMoneyInfo> moneyItem = new List<GMoneyInfo>();
            ItemData temp = new ItemData();
            if (GameMysql.instance.IsTableExists(SQLTable.ItemTable))
            {
                GameMysql.instance.SelectTable(SQLTable.ItemTable, (data, isOk) =>
                {
                    if (isOk)
                    {
                        ItemData item = new ItemData();
                        item.id = data.GetInt32(nameof(item.id));
                        item.num = data.GetInt32(nameof(item.num));
                        moneyItem.Add(item);
                    }
                });
            }
            else
            {
                string sqlStr = $"{nameof(temp.id)} INT,{nameof(temp.num)} INT,PRIMARY KEY({nameof(temp.id)})";
                GameMysql.instance.CreateTable(SQLTable.ItemTable, sqlStr);
                for (int i = 1; i <= 10; i++)
                {
                    string key = $"{nameof(temp.id)},{nameof(temp.num)}";
                    string value = $"{i},{0}";
                    GameMysql.instance.InsertTable(SQLTable.ItemTable, key, value);
                    ItemData item = new ItemData();
                    item.id = i;
                    item.num = 0;
                    moneyItem.Add(item);
                }
            }

            return moneyItem;
        }
    }
}
