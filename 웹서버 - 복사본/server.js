const WebSocket = require('ws')
const express = require("express")
const util = require('util')
const path = require('path');
const bodyParser = require("body-parser")
const app = express()

var db = require('./bin/DB.js');
const { connect } = require('http2');
const { debug, Console } = require('console');
const { spawn } = require('child_process');
const { queryObjects } = require('v8');
const json = require('body-parser/lib/types/json.js');

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: false }));
app.use(express.static("public"))
db.query = util.promisify(db.query);

var userId
var enemyList = []
var userList = []

app.listen(8001, () => {
    console.log(`http sever port 8001`)
})

app.post('/getData', (req, res) => {
    
    var result = req.body // mapName,  mapSize(x , y) , mapData(List) , DecoData(List) , EnemyData(List)
    var query = 'INSERT INTO mapData (mapSizeX , mapSizeY , mapValue , mapName , decoValue , enemyValue , colliderValue , npcvalue) Values (? , ? , ? ,? , ? , ? , ? , ?)';
    var params = [result.mapSize.x, result.mapSize.y, JSON.stringify(result.mapData), result.mapName, JSON.stringify(result.DecoData), JSON.stringify(result.EnemyData) , JSON.stringify(result.ColliderData) , JSON.stringify(result.NPCData)];
    db.query(query, params, function (err, rows, fields) {
        if (err) console.log(err)
    });
})

app.post('/inventoryData', (req, res) => {
    var result = req.body;
    console.log(result);
    let who = userStateChange(result)
    var query = 'Select * from user_inventory where id = ?'
    var params = [userList[who].name]
    
    db.query(query, params, function (err, rows, fields) {
        var data = {
            item: rows[0].item,
            gold: rows[0].gold,
            equip : rows[0].equipItemSlot,
            equipItemTab : rows[0].equipItemTab
        }
        res.send(data);
    })
})

app.post('/purchaseSkill' , (req, res) => {
    var skillData = JSON.parse(req.body.skillType);;
    var who = userStateChange(skillData);
    var insertquery = `update user_status set learned_skill = json_merge_preserve(learned_skill , JSON_ARRAY(?)) where id = ?`
    var insertparmas = [skillData.skill_type , userList[who].name]
    
    var gold = `select * from user_inventory where id = ?`;
    var params = userList[who].name;

    db.query(gold , params , function(err, rows) {
        console.log(skillData);
        var change = `update user_inventory set gold = ? where id = ?`;
        var changeparams = [rows[0].gold - skillData.learnableGold , userList[who].name];
        db.query(change , changeparams);
    })
    db.query(insertquery , insertparmas , function(err) {
        if(err) res.send('insert fail');
        else res.send(`insert sussecs\n **Data Type = ${skillData.skillType} , ${userList[who]}`);
    })
    
    
})
app.post('/signup' , (req , res) => {
    var result = JSON.parse(req.body.signData);
    var query = "insert into user_info (id , password) values (? , ?)";
    var params = [result.ID , result.Password];

    db.query(query , params , function(err) {
        if(err) res.send("회원가입 실패");
        else res.send("회원가입 성공");
    })
})
app.post('/getSkillData' , async (req , res) => {
    var result = JSON.parse(req.body.needSkill);

    var getSkillquery = "select * from skill where learnable_character = ?";
    var getSkillparams = result.type;

    let who = userStateChange(result);
    var getSkilllearndquery = "select learned_skill from user_status where id = ?";
    var getSkilllearndparams = userList[who].name;
   
    const skillData = await db.query(getSkillquery , getSkillparams);
    const learnedSkillData = await db.query(getSkilllearndquery , getSkilllearndparams);
    var sendData = {
        skillsData : skillData,
        skillLearn : learnedSkillData
    };

    res.send(sendData)
})
app.post('/saveEquipItem' , (req , res) => {
    var result = JSON.parse(req.body.Equip);
    let who = userStateChange(result);
    var query = "update user_inventory set equipItemSlot = Json_Set(equipItemSlot , ? , ?) where id = ?";
    var params = [`$."${result.Key}"` , result.Value[0] , userList[who].name];

    db.query(query , params , function(err , rows) {
        
        if(userList[who].equipItem[result.Key] != 0){
            //userList[who].attack -= itemList[userList[who].equipItem[result.Key] - 1].item_damage;
            userList[who].defense -= itemList[userList[who].equipItem[result.Key] - 1].item_defense;
        }
        
        userList[who].equipItem[result.Key] = result.Value[0];
        
        if(result.Value[0] != 0){
            //userList[who].attack += itemList[result.Value[0] - 1].item_damage;
            userList[who].defense += itemList[result.Value[0] - 1].item_defense;
        }
       
        console.log(`User attack : ${userList[who].attack}  User defense : ${userList[who].defense}`);
    })
})

app.post('/saveEquipItemTab' , (req , res) => {
    var result = JSON.parse(req.body.EquipItemTab);
    let who = userStateChange(result);
    console.log("Get Change Equip Item Slot Data is : ")
    console.log(result);
    var query = "update user_inventory set equipItemTab = JSON_SET(equipItemTab , ? , ?) where id = ?";
    var params = [`$."${result.Key}"` , result.ItemSlotIndex , userList[who].name];

    db.query(query , params , function(err) {
        if(err) console.log(err)
        else console.log(`Save EquipItemTab\n -UpdateData is {${result.Key} : ${result.ItemSlotIndex}} ChangeUser is  ${userList[who].name}`)
    })
})
app.post('/getItemData' , (req , res) => {
    var result = req.body;
    res.send({itemInfos : itemList});
})
app.post('/saveinventoryData', (req, res) => {
    var result = req.body;
    result = JSON.parse(result.item);
    let who = userStateChange(result);
    var query = 'update user_inventory set item = JSON_SET(item , ? , JSON_ARRAY(?)) where id = ?'
    var params = [`$."${result.Key}"`, result.Value, userList[who].name];

    db.query(query, params, function (err, rows, fields) {
        if (err) err
        res.send("su")
    });

})
app.post('/mapData', (req, res) => {
    var result = req.body;
    var query = 'SELECT * FROM MAPDATA WHERE MAPNAME = ?';
    var parmas = result.NeedMapName;
    thisMapEnemyList = [];
    thisMapUserList =[];
    db.query(query, parmas, function (err, rows, fields) {
        var data = {
            "mapData": rows
        }
        firstMapEnemy[rows[0].mapName].forEach(enemy => {
            thisMapEnemyList.push(enemyList[enemy])
        });
        for(let i = 0 ; i < userList.length; i++){
            if(userList[i].mapName == rows[0].mapName){
                thisMapUserList.push(userList[i])
            }
        }
        res.send(data);
    })
})
app.post('/getQuestData' , (req , res) => {
    query = 'SELECT * FROM QUEST';
    console.log("get Quest");
    db.query(query , function(err, rows , fields) {
        var data = {
            quests : rows
        }
        res.send(JSON.stringify(data));
    })
})
var id;

app.post('/login', (req, res) => {
    var result = req.body.Login;
    var userInfo = JSON.parse(result);
    id = userInfo.ID;
    var user_Id_search_query = "SELECT id from user_info WHERE ID = ?";
    var user_Id_search_params = id;

    var user_password_search_query = "SELECT * FROM user_info WHERE ID = ?";
    db.query(user_Id_search_query, user_Id_search_params, function (err, rows, fields) {
        if (rows.length == 0) {
            res.send("없는 아이디입니다.");
        } else {
            db.query(user_password_search_query, user_Id_search_params, function (err, rows, fields) {
                if (rows[0].password == userInfo.Password) {
                    if (rows[0].nick_name == null) {
                        res.send("닉네임 입력.")
                    } else {

                        userId = rows[0].nick_name;
                        res.send("로그인 성공.");

                    }

                } else {
                    res.send("틀린 비밀번호 입니다.");
                }
            })
        }
    })
})

app.post('/setNickName', (req, res) => { 
    var result = JSON.parse(req.body.NickName);
    var query = "Update user_info set nick_name = ? , character_type = ? where id = ?";
    var params = [result.nick_name, result.characterType ,id];

    var insertquery = "Insert into user_status (id , hp , mp , strStats , intStats , attackRadious , learned_skill) values (? , ? , ? , ? , ? , ? , JSON_ARRAY())";

    var findcharacterInfoquery = "select * from character_type where type = ?"

    var inventoryquery = "Insert Into user_inventory (id , item , gold , EquipItemSlot , equipItemTab) values (? , ? , ? , ? , ?)";
    var inventoryparams = [id, JSON.stringify({}), 0 , JSON.stringify({}) , JSON.stringify({})]

    var inventoryInitQuery = "UPDATE user_inventory SET item = JSON_SET(item"
    for (let i = 1; i <= 30; i++) {
        inventoryInitQuery += `, '$."${i}"', JSON_ARRAY(0,0)`;
    }
    inventoryInitQuery += ') WHERE id = ?';
    inventoryInitParams = [id]

    var equipList = ['Ring' , 'Armor' , 'Shoes' , 'Gloves' , 'Helmet' , 'Shield' , 'Weapon' , 'Bracelet'];
    var equipItemTabQuery = "Update user_inventory set equipItemTab = Json_set(EquipItemTab"; 
    var equipItemInitQuery = "Update user_inventory set EquipItemSlot = Json_set(EquipItemSlot"
    for(let i = 0; i < equipList.length; i++){
        equipItemTabQuery += `, '$."${equipList[i]}"' , 0`;
        equipItemInitQuery += `, '$."${equipList[i]}"' , 0`;
    }
    equipItemTabQuery  += ') where id = ?';
    equipItemInitQuery += ') where id = ?';

    equipItemInitParams = [id];
    
    db.query(query, params, function (err, rows, fields) {
        if (err) {
            console.log(err)
        }
        console.log("user_info 업데이트 성공");
        userId = params[0];

        
        db.query(inventoryquery, inventoryparams, function (err, rows, fields) {
            if (err) {
                console.log(err)
            }
            console.log("Inventory 생성 완"); 
            db.query(inventoryInitQuery, inventoryInitParams);
            db.query(equipItemInitQuery , equipItemInitParams);
            db.query(equipItemTabQuery , equipItemInitParams);
        })
        db.query(findcharacterInfoquery , result.characterType , function (err , rows , fields) {
            var insertparmas = [id, rows[0].maxhp, rows[0].maxmp , 1, 1 , rows[0].attackRadious , ]

            db.query(insertquery, insertparmas, function (err, rows, fields) {
                if (err) {
                    console.log(err)
                }
    
                console.log("UserData 생성 완");
                res.send("로그인 성공.")
            })
        })

    })


})


const wss = new WebSocket.Server({ port: 8000 }, () => {
    console.log('wss server port 8000')
})
let RespawnInterval = new Map();
EnemyInit()
ItemListInit()
var itemList = [];
var firstMapEnemy = {};
var NpcSpawn = {};
var NPCList = [];
var thisMapEnemyList = [];
var thisMapUserList = [];
wss.on('connection', async (ws, req) => {
    
    
    await init(ws);
    console.log(NPCList);
    let who = userStateChange(ws);
    var data1 = JSON.stringify({ 
        title: "Init",
        id: userId,
        this_player: userList[who],
        enemyList: enemyList,
        NPC : [{NPCList : NPCList}]
    });
    ws.send(data1);
    ws.on('close', () => {
        for (let i = 0; i < userList.length; i++) {
            if (userList[i].id == ws.id) {
                userList.splice(i, 1);

            }
        }
        for (let i = 0; i < enemyList.length; i++) {
            if (enemyList[i].FollowTarger != null && enemyList[i].FollowTarger.id == ws.id) {
                enemyList[i].FollowTarger = null;
                enemyList[i].state = "MoveAround"
            }
        }

        var data2 = {
            title: "CheckOutUser",
            users: userList,
            id: ws.id
        }
        all_player_response(data2)
    })
    ws.on('message', function message(data) {

        data = JSON.parse(data)
        if(data.title == "CheckThisMapEnemy"){
            let who = userStateChange(ws);

            ws.send(JSON.stringify({title : "CheckThisMapEnemy" , enemyList : thisMapEnemyList , users : thisMapUserList , NPC : NpcSpawn[userList[who].mapName]}));
            wss.clients.forEach(function each(client) {
                if(ws.id != client.id) {
                    client.send(JSON.stringify({title : "UserChangeMap" , this_player : userList[who]}))
                }
            })
        }
        if(data.title == "chatting") {
            let who = userStateChange(ws)
            var data = {
                title : "ChattingManager" , 
                this_player : userList[who],
                chattingText : data.chattingText
            }
            all_player_response(data)
        }
        if(data.title == "changeMap"){
            var result = data;
            var query = 'SELECT * FROM MAPDATA WHERE MAPNAME = ?';
            var parmas = result.mapName;
            thisMapEnemyList = [];
            thisMapUserList = [];
            
            db.query(query, parmas, function (err, rows, fields) {
                
                let who = userStateChange(ws);
                userList[who].mapName = result.mapName;

                for(let i = 0; i < firstMapEnemy[rows[0].mapName].length; i++){
                    thisMapEnemyList.push(enemyList[i]);
                }                
                
                for(let i = 0 ; i < userList.length; i++){
                    if(userList[i].mapName == rows[0].mapName){
                        thisMapUserList.push(userList[i])
                    }
                }
 
                wss.clients.forEach(function each(client) {
                    if(client.id == ws.id){
                        console.log(ws.id + "changeMap 에게 데이터 전송")
                        ws.send(JSON.stringify({
                            title : "changeMap" ,
                            mapData : rows
                        }))
                    }
                })
            })
        }
        if(data.title == "clearQuest") {
            let who = userStateChange(ws);
            
            userList[who].gold += data.clearGold;
            userList[who].clearquestnumber++;
            CheckExp(userList[who] , data.clearExp , ws);
            // 유저의 clearQuestNumber를 데이터베이스에 플러스된 클리어 넘버를 업데이트하는 로직
            const userClearQuestNumberPlus = "Update user_status set clearquestnumber = ? where id = ?";
            const params = [userList[who].clearquestnumber , userList[who].name];
            db.query(userClearQuestNumberPlus , params , function(err) {
                if(err) console.log(err);
                else {
                    data = {
                        title : "ClearQuestReward",
                        this_player : userList[who],
                        useItemType : data.clearGold,
                        dropItem : data.clearItem
                    }
                    ws.send(JSON.stringify(data));
                }
            })

        }
        if(data.title == "StatusUpgrade") {
            let who = userStateChange(ws);
            let query = "Update user_status set strStats = ? , intStats = ? where id = ?"
            let params = [data.this_player.strStats , data.this_player.intStats , userList[who].name]
            userList[who].strStats = data.this_player.strStats;
            userList[who].intStats = data.this_player.intStats;

            db.query(query , params , function(err) {
                
                console.log(`${data.this_player.id} 의 stat이 업데이트 되었습니다`);
                console.log(`힘스탯 : ${data.this_player.strStats} 지능스탯 : ${data.this_player.intStats}`);
            })

            var data1 = {
                title : "UpgradeStat" ,
                id : "Str"  
            }
            ws.send(JSON.stringify(data1));
        }
        if (data.title == "UsePostion"){
            let who = userStateChange(data);
            console.log("Use Postion ");
            userList[who].hp += itemList[data.Value[0] - 1].HPrecovery;
            userList[who].mp += itemList[data.Value[0] - 1].MPrecovery;
            
            var query =  "Update user_inventory set item = JSON_SET(item , ? , JSON_ARRAY(? , ?)) where id = ?";
            var params = [`$."${data.Key}"` , data.Value[0] , data.Value[1] , userList[who].name];

            db.query(query , params);

            all_player_response({title : "CheckPlayerHP" , this_player : userList[who]})
        }
        if(data.title == "UseSkill") {
            let who = userStateChange(data);
            userList[who].mp -= data.useItemType;
            var sendUser = {title : "UseSkill" , id : data.id , this_player : userList[who] ,  skillinfo : data.skillinfo};
            all_player_response(sendUser);
        }
        if (data.title == "SaveData") {
            let who = userStateChange(data.this_player);
            var query = "UPDATE user_status Set strstats = ? , intstats = ? , exp = ? , Level = ? where id = ?";
            console.log( userList[who].name);
            var params = [data.this_player.strStats, data.this_player.intStats, data.this_player.exp, data.this_player.Level, userList[who].name];

            db.query(query, params, function (err, rows, fields) {
                console.log("데이터 저장 완료");
            })
        }
        if (data.title == "Connection") {
            var data2 = { title: "CreateOtherUser", users: userList }
            all_player_response(data2);
        }
        if (data.title == "PlayerMove") {
            var data2 = { title: "CheckMove", id: ws.id, x: data.x, y: data.y, moveXY: data.moveXY }
            let who = userStateChange(ws);

            userList[who].x = data.x;
            userList[who].y = data.y;

            without_player_response(data2, ws)
        }
        if (data.title == "GetCoin") {
            let who = userStateChange(ws);
            var query = "select * from user_inventory where id = ?";
            var params = [userList[who].name]
            db.query(query, params, function (err, rows, fields) {
                var result = rows[0].gold + data.useItemType;
                var query = "Update user_inventory set gold = ? where id = ?";
                var params = [result, userList[who].name]
                console.log("getGold");
                db.query(query, params)
                
            })            
        }

        if(data.title == "BuyItem") { 
            let who = userStateChange(ws);
            var query = "select * from user_inventory where id = ?";
            var params = [userList[who].name]
            db.query(query, params, function (err, rows, fields) {
                if(err) err;
                console.log("buy item");
                var result = rows[0].gold - itemList[data.id - 1].cost;
                var query = "Update user_inventory set gold = ? where id = ?";
                var params = [result, userList[who].name]

                db.query(query, params);
                /*
                var data1 = {
                    title : "BuyItem",
                    id : ws
                }
                
                ws.send(JSON.stringify(data1))
                */
            })  
            
            
        }
        if (data.title == "HitEnemy") {
            enemyList.forEach(element => {
                if (element.id == data.id) {
                    element.Hp -= data.hitDamage;
                    if (element.Hp < 0) {
                        element.state = "Die"
                        let who = userStateChange(data.this_player);
                        
                        CheckExp(userList[who] , element.DropExp , ws);
                       
                        //fix !
                        var isDropItem = GetRandomInt(0, 100)

                        var DropItem = 0;

                        if (isDropItem < 20){
                            DropItem = element.dropItemList[Math.floor(Math.random() * element.dropItemList.length)];
                        }

                        if (!element.isDie) {
                            let RespawnId = setInterval(() => { EnemyRespawn(element) }, 5000);
                            RespawnInterval.set(element.id, RespawnId);
                            element.isDie = true;
                        }
                        element.FollowTarger = null;
                        all_player_response({ title: "EnemyHit", enemy: element, this_player: userList[who] , hitDamage : data.hitDamage});
                        all_player_response({ title: "EnemyDie", enemy: element, this_player: userList[who], dropItem: DropItem , hitDamage : data.hitDamage})
                    } else {
                        element.state = "Hit"
                        all_player_response({ title: "EnemyHit", enemy: element, this_player: userList[who] , hitDamage : data.hitDamage});
                    }

                }
            });

        }

        if (data.title == "AttackOtherPlayer") {
            var data2 = {
                title: "CheckAttack",
                id: data.id,
            }
            without_player_response(data2);
        }
    })
})

wss.on('listening', () => {
    console.log('리스닝')
})

setInterval(EnemyChangeState, 1000)

let AttackInterval = new Map();

function CheckExp(player , exp , ws){
    player.exp += exp;
    if(player.exp >= player.maxExp){
        player.Level++;
        player.exp -= player.maxExp;
        player.maxExp = player.Level * 100;
    } 
}

function EnemyChangeState() {
    for (let i = 0; i < enemyList.length; i++) {
        if(enemyList[i].FollowTarger != null){
            for(let j = 0; j < userList.length; j++){
                if(userList[j].id == enemyList[i].FollowTarger.id){
                    if(userList[j].mapName != enemyList[i].EnemySpawnMapName) {
                        enemyList[i].FollowTarger = null;
                        enemyList[i].state = "MoveAround";
                        break;
                    }
                }
            }
        }

        //console.log("┌───────────────────────────────────────┐")
        if (enemyList[i].state != "Hit" && enemyList[i].state != "Die") {
            if (enemyList[i].FollowTarger == null) { // 유저 찾아오기
                userList.forEach(user => {
                    if (enemyList[i].EnemySpawnMapName == user.mapName) {
                        let dx = user.x - enemyList[i].x
                        let dy = user.y - enemyList[i].y
                        //console.log(`user<->enemy${i} Distance ${Math.abs((user.x - enemyList[i].x) + (user.y - enemyList[i].y))}`);
                        if (Math.sqrt(dx * dx + dy * dy) < 5) {
                            enemyList[i].FollowTarger = user;
                        }
                    }
                });
            }
            
            if (enemyList[i].FollowTarger != null) { // 유저가 비어있지 않으면 그 유저한테로의 상태 설정
               
                let dx = enemyList[i].FollowTarger.x - enemyList[i].x
                let dy = enemyList[i].FollowTarger.y - enemyList[i].y

                if (Math.sqrt(dx * dx + dy * dy) > 5) {
                    enemyList[i].FollowTarger = null;
                    enemyList[i].state = "MoveAround"
                }
                else if (Math.sqrt(dx * dx + dy * dy) < 1) {
                    enemyList[i].state = "AttackAroundInUser"
                    if (!enemyList[i].isAttack) {
                        let AttackId = setInterval(() => { EnemyAttack(enemyList[i]) }, enemyList[i].AttackTime * 1000);
                        AttackInterval.set(enemyList[i].id, AttackId);
                        enemyList[i].isAttack = true;
                    }
                } else if (Math.sqrt(dx * dx + dy * dy) < 5) {
                    enemyList[i].state = "FollowUser";
                }

            }
            if (enemyList[i].state != "AttackAroundInUser") {
                if (AttackInterval.has(enemyList[i].id)) {
                    clearInterval(AttackInterval.get(enemyList[i].id))
                    AttackInterval.delete(enemyList[i].id)
                    enemyList[i].isAttack = false;
                }
            }
            if (enemyList[i].state == "MoveAround") {
                if (enemyList[i].spawnPos.x + 2 < enemyList[i].x || enemyList[i].spawnPos.y + 2 < enemyList[i].y) {
                    enemyList[i].movePos = -1;
                }
                else if (enemyList[i].spawnPos.x - 2 > enemyList[i].x || enemyList[i].spawnPos.y - 2 > enemyList[i].y) {
                    enemyList[i].movePos = 1;
                }
                //console.log(`${i} spawnPos : ${enemyList[i].spawnPos.x}  ${enemyList[i].x}  ${enemyList[i].y}  movePos : ${movePos}`)

                enemyList[i].x += (Math.random() * enemyList[i].movePos);
                enemyList[i].y += (Math.random() * enemyList[i].movePos);
            } else if (enemyList[i].state == "FollowUser") {

                enemyList[i].x += (enemyList[i].FollowTarger.x - enemyList[i].x) * 0.3
                enemyList[i].y += (enemyList[i].FollowTarger.y - enemyList[i].y) * 0.3

                //console.log(` user<->enemy Distance ${Math.sqrt((dx * dx)+(dy * dy))}`)

                for (let j = 0; j < enemyList.length; j++) {
                    if (i == j) {
                        continue;
                    }
                    let enemyDx = enemyList[i].x - enemyList[j].x;
                    let enemyDy = enemyList[i].y - enemyList[j].y;
                    //console.log("enemy"+i + "  enemy" + j+ "  Distance " + Math.sqrt((enemyDx * enemyDx) + (enemyDy * enemyDy)));
                    if (Math.sqrt((enemyDx * enemyDx) + (enemyDy * enemyDy)) < 0.5) {
                        enemyList[i].x += Math.random() * 2 - 1
                        enemyList[i].y += Math.random() * 2 - 1
                    }
                }
            }
        } else if (enemyList[i].state == "Hit") {
            enemyList[i].state = "MoveAround";
        }

        //console.log(`\t${i} EnemyState : ${enemyList[i].state}`)
        //console.log(`\t     EnemyHp : ${enemyList[i].Hp}`)

        //console.log("└───────────────────────────────────────┘")
    }
    //console.log("\n\n\n\n\n\n\n\n")

    wss.clients.forEach(function (client) {
        client.send(JSON.stringify({ title: "EnemyAround", enemyList: enemyList }));
    })
}
function EnemyAttack(enemy) {
    if(enemy.FollowTarger != null){
        let who = userStateChange(enemy.FollowTarger);
        let damgeValue = 0;
        if(userList[who].defense >= enemy.damage){ 
            damgeValue = 1
            userList[who].hp -= damgeValue;

        }
        else {
            damgeValue = enemy.damage - userList[who].defense;
            userList[who].hp -= damgeValue;
        }
        if(userList[who].hp <=0 ){
            userList[who].hp = 0;
        }


        all_player_response({ title: "EnemyAttack", enemy: enemy , this_player : userList[who] , hitDamage : damgeValue})
    }
}
function EnemyRespawn(enemy) {
    enemy.state = "MoveAround";
    enemy.Hp = enemy.MaxHp;
    enemy.x = enemy.spawnPos.x
    enemy.y = enemy.spawnPos.y

    wss.clients.forEach(function each(client) {
        client.send(JSON.stringify({ title: "EnemyRespawn", enemy: enemy }));
    })
    enemy.isDie = false;
    clearInterval(RespawnInterval.get(enemy.id));
    RespawnInterval.delete(enemy.id)

}

function all_player_response(data) {
    wss.clients.forEach(function each(client) {
        client.send(JSON.stringify(data))
    })
}
function without_player_response(data) {
    wss.clients.forEach(function each(client) {
        if (data.id != client.id) {
            client.send(JSON.stringify(data))
        }
    })
}
async function EnemyInit() {
    try {
        let EnemySpawnPos = {};

        var query = "select mapSizeX, mapSizeY, mapName, enemyValue , npcValue from mapData";
        var query1 = "select * from enemy_info where type = ?";
        var query2 = "select * from npc_info where type = ?";
        var enemyID = 0;
        var NPCID = 0;
        
        const rows = await db.query(query);

        for (let i = 0; i < rows.length; i++) {
            var bottomX = -rows[i].mapSizeX / 2;
            var bottomY = -rows[i].mapSizeY / 2;
            let xPos = 0; 
            let yPos = 0;
            if(!(rows[i].mapName in firstMapEnemy)){
                firstMapEnemy[rows[i].mapName] = [];    
            }
            if(!(rows[i].mapName in NpcSpawn)) {
                NpcSpawn[rows[i].mapName] = [];
            }

            for (let j = 0; j < rows[i].enemyValue.length; j++) {
                if (xPos % rows[i].mapSizeX === 0 && xPos !== 0) {
                    yPos++;
                }
                var positionX = xPos % rows[i].mapSizeX + bottomX;
                var positionY = yPos + bottomY;
               
                
                if(rows[i].npcValue[j] != 0) {
                    const npcRows = await db.query(query2 , [rows[i].npcValue[j]]);
                    NPCList.push({id : NPCID , type : rows[i].npcValue[j] ,  npc_type : npcRows[0].npc_type , npc_image : npcRows[0].npc_image});
                    NpcSpawn[rows[i].mapName].push({id : NPCID , type : npcRows[0].type , name : npcRows[0].name ,spawnPos : {x : positionX, y : positionY} , talk : npcRows[0].talk , sellingList : npcRows[0].sellingList , quest_type : npcRows[0].quest_type , mapName : rows[i].mapName})
                    NPCID++;
                }

                if (rows[i].enemyValue[j] != 0) {
                    const enemyRows = await db.query(query1, [rows[i].enemyValue[j]]);
                
                    enemyList.push({
                        id: enemyID,
                        damage : enemyRows[0].damage,
                        type: enemyRows[0].type,
                        x: positionX,
                        y: positionY,
                        spawnPos: { x: positionX, y: positionY },
                        FollowTarger: null,
                        movePos: 1,
                        state: "MoveAround",
                        Hp: enemyRows[0].maxHp,
                        MaxHp: enemyRows[0].maxHp,
                        AttackTime: enemyRows[0].attackTime,
                        DropExp: enemyRows[0].dropExp,
                        DropGold: enemyRows[0].dropGold,
                        EnemySpawnMapName: rows[i].mapName,
                        isDie: false,
                        isAttack: false,
                        mapName : rows[i].mapName,
                        dropItemList : enemyRows[0].dropItemList
                    });
                    firstMapEnemy[rows[i].mapName].push(enemyID);
                    enemyID++;
                }
                xPos++;
                
            }
        }
    } catch (err) {
        console.error(err);
    }
}
/*
async function EnemyInit() {
    let EnemySpawnPos = {};

    var query = "select mapSizeX , mapSizeY , mapName , enemyValue from mapData"
    var query1 = "select * from enemy_info where type = ?";
    await db.query(query , function(err , rows , fields){
        var result = rows;
        for(let i = 0 ; i < rows.length; i++){
            let xPos = 0;
            let yPos = 0;
            var id = 0;
            for(let j = 0; j < rows[i].enemyValue.length; j++){
                if(xPos % rows[i].mapSizeX == 0 && xPos != 0){
                    yPos++;
                    xPos = 0;
                }
                xPos++;
                if(rows[i].enemyValue[j] == 0) continue;

                await db.query(query1 , rows[i].enemyValue[j] , function(err , rows , fields) {
                    enemyList.push({
                        id : id,
                        type : rows[0].type,
                        x: xPos,
                        y: yPos,
                        spawnPos: { xPos, yPos },
                        FollowTarger: null,
                        movePos: 1,
                        state: "MoveAround",
                        Hp: rows[0].maxHp,
                        MaxHp: rows[0].maxHp,
                        AttackTime: rows[0].attackTime,
                        DropExp: rows[0].dropExp,
                        DropGold: rows[0].dropGold,
                        EnemySpawnMapName : result[i].mapName,
                        isDie: false,
                        isAttack : false
                    })
                    id++;
                })
                
            }

        }
    })
   */

/*for (let i = 0; i < 3; i++) {
    let x = 5;
    let y = 3 * i + 0.5;
    enemyList.push({
        id: i,
        type: 1,
        x: 5,
        y: 3 * (i + 0.5),
        spawnPos: { x, y },
        FollowTarger: null,
        movePos: 1,
        state: "MoveAround",
        Hp: 100,
        MaxHp: 100,
        AttackTime: 2,
        DropExp: 50,
        isDie: false,
        isAttack : false
    })
}
 
}
*/
function userStateChange(ws) {
    for (let i = 0; i < userList.length; i++) {

        if (userList[i].id == ws.id) {
            return i;
        }
    }
    return 'fail'
}
async function ItemListInit(){
    var query = "select * from item_info";
    const itemDatas = await db.query(query);
    for(let i = 0; i < itemDatas.length; i++){
        itemList.push(itemDatas[i]);
    }

}
function init(ws) {
    return new Promise((resolve, rejects) => {
        
        var getUserDataquery = "select user_status.clearquestnumber ,user_info.id , attackRadious , user_info.character_type ,hp , mp , strStats , intStats , exp , nick_name , maxHp, user_inventory.equipItemSlot , Level from user_status , user_info , user_inventory where user_status.id = ? && user_info.id = ? && user_inventory.id = ?";
        var getUserDataparams = [id, id , id];

        db.query(getUserDataquery, getUserDataparams, function (err, rows, fields) {
            let defenseValue = 0;
            //let attackValue = 0;
            for(var key in rows[0].equipItemSlot){
                if(rows[0].equipItemSlot[key] != 0) {
                    //attackValue += itemList[rows[0].equipItemSlot[key] - 1].item_damage ;
                    defenseValue += itemList[rows[0].equipItemSlot[key] - 1].item_defense;
                }
            }   

            
            userList.push({
                type : rows[0].character_type,
                name: rows[0].id,
                id: rows[0].nick_name,
                x: 0,
                y: 0,
                maxhp : rows[0].maxHp,
                hp: rows[0].hp,
                mp: rows[0].mp,
                strStats: rows[0].strStats,
                intStats: rows[0].intStats,
                exp: rows[0].exp,
                Level: rows[0].Level,
                maxExp: rows[0].Level * 100,
                mapName: "상점",
                equipItem : rows[0].equipItemSlot,
                defense : defenseValue,
                //attack : attackValue + (rows[0].strStats * 10),
                clearquestnumber : rows[0].clearquestnumber,
                attackRadious : rows[0].attackRadious
            })

            ws.id = rows[0].nick_name;

            resolve();
        })

    })
}
function GetRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min)) + min;
} 