const WebSocket = require('ws')
const express = require("express")
const path = require('path');
const bodyParser = require("body-parser")
const app = express()

var db = require('./bin/DB.js');
const { connect } = require('http2');
const { debug } = require('console');
const { spawn } = require('child_process');

app.use(bodyParser.json()); 
app.use(bodyParser.urlencoded({ extended: false }));
app.use(express.static("public"))


var userId
var enemyList = []
var userList = []

app.listen(8001, () => {
    console.log(`http sever port 8001`)
})

app.post('/getData' , (req,res) => {
    var result = req.body

    var query = 'INSERT INTO mapData (mapSizeX , mapSizeY , mapValue , mapName , decoValue) Values (? , ? , ? ,? , ?)';
    var params = [result.mapSize.x , result.mapSize.y , JSON.stringify(result.mapData) , result.mapName , JSON.stringify(result.DecoData)];
    db.query(query , params , function(err , rows , fields){
        if(err) console.log(err)
    });

})


app.post('/mapData' , (req, res) => {
    var reslut = req.body;
    var query = 'SELECT * FROM MAPDATA WHERE MAPNAME = ?';
    var parmas = reslut.NeedMapName;
    db.query(query , parmas , function(err , rows , fields) {
        var data = {
            "mapData" : rows
        }
        res.send(data);
    })
})
var id;

app.post('/login' , (req , res) => {
    var result = req.body.Login;
    var userInfo = JSON.parse(result);  
    id = userInfo.ID;
    var user_Id_search_query = "SELECT id from user_info WHERE ID = ?";
    var user_Id_search_params = id;

    var user_password_search_query = "SELECT * FROM user_info WHERE ID = ?";
    db.query(user_Id_search_query , user_Id_search_params , function(err , rows , fields) {
        if(rows.length == 0){
            res.send("없는 아이디입니다.");
        } else {
            db.query(user_password_search_query , user_Id_search_params , function(err , rows , fields) {
                if(rows[0].password == userInfo.Password){
                    if(rows[0].nick_name == null){
                        res.send("닉네임 입력.")
                    }else {
                        userId = rows[0].nick_name;
                        res.send("로그인 성공.");
                        
                    }
                    
                }else {
                    res.send("틀린 비밀번호 입니다.");
                }
            })        
        }
    })
})

app.post('/setNickName' , (req , res) => {
    var reslut = req.body;
    console.log(reslut);
    var query = "Update user_info set nick_name = ? where id = ?";
    var params = [reslut.NickName , id];

    var insertquery = "Insert into user_status (id , hp , mp , strStats , intStats) values (? , ? , ? , ? , ?)";
    var insertparmas = [id , 100  , 100 , 1 , 1]

    
    db.query(query , params , function(err , rows , fields) {
        if(err){
            console.log(err)
        }
        console.log("user_info 업데이트 성공");
        userId = params[0];
        db.query(insertquery , insertparmas , function(err , rows , fields) {
            if(err){
                console.log(err)
            }
            console.log("Insert 완");
            res.send("로그인 성공.")
        })
    })


})


const wss = new WebSocket.Server({port : 8000}, () => {
    console.log('wss server port 8000')
})

EnemyInit()

wss.on('connection', async (ws , req) => {
    
    await init(ws);

    let who = userStateChange(ws);
    console.log(userList[who])
    var data1 = JSON.stringify({
        title : "Init" ,
        id : userId ,
        this_player : userList[who] ,
        enemyList : enemyList
    });
    console.log(userList)
    ws.send(data1);

    ws.on('close' , () => {
        for(let i = 0; i < userList.length; i++){
            if(userList[i].id == ws.id){
                userList.splice(i,1);
                
            }
        }
        for(let i = 0; i < enemyList.length; i++){
            if(enemyList[i].FollowTarger != null && enemyList[i].FollowTarger.id == ws.id){
                enemyList[i].FollowTarger = null;
                enemyList[i].state = "MoveAround"
            }
        }
        
        var data2 = {
            title : "CheckOutUser" ,
            users : userList,
            id : ws.id
        }
        all_player_response(data2)
    })
    ws.on('message', function message(data) {
    
        data = JSON.parse(data)
        if(data.title == "SaveData"){;
            let who = userStateChange(ws);
            var query = "UPDATE user_status Set hp = ? , mp = ? , strstats = ? , intstats = ? , exp = ? , Level = ? where id = ?";
            var params = [data.this_player.hp , data.this_player.mp , data.this_player.strStats , data.this_player.intStats , data.this_player.exp , data.this_player.Level , userList[who].name];

            db.query(query , params , function(err , rows ,fields) {
                console.log("데이터 저장 완료");
            })
        }
        if(data.title == "Connection"){
            var data2 = {title : "CreateOtherUser" , users : userList}
            all_player_response(data2);
        }
        if(data.title == "PlayerMove"){
            var data2 = {title : "CheckMove" , id : ws.id ,x : data.x , y : data.y , moveXY : data.moveXY}
            
            let who = userStateChange(ws);
            userList[who].x = data.x;
            userList[who].y = data.y;

            without_player_response(data2 , ws)
        }
         
        if(data.title == "HitEnemy"){
            enemyList.forEach(element => {
                if(element.id == data.id){
                    element.Hp -= 10 * data.this_player.strStats;
                    if(element.Hp < 0){
                        element.state = "Die"
                        data.this_player.exp += element.DropExp;
                        if(data.this_player.exp >= data.this_player.maxExp){
                            data.this_player.Level++;
                            data.this_player.exp -= data.this_player.maxExp;
                            data.this_player.maxExp = data.this_player.Level * 100;
                        }
                        element.FollowTarger = null;
                        console.log(data.this_player);
                    }else {
                        element.state = "Hit"
                    }

                }
            });
            all_player_response({title : "EnemyAround" ,  enemyList : enemyList , this_player : data.this_player});
        }

        if(data.title == "AttackOtherPlayer"){
            var data2 = {
                title : "CheckAttack",
                id : data.id,
            }
            without_player_response(data2);
        }
})
})

wss.on('listening' , () => {
    console.log('리스닝')
})

setInterval(EnemyChangeState , 1000)
setInterval(EnemyRespawn , 5000)
function EnemyChangeState(){
      for(let i = 0; i < enemyList.length; i++){
        console.log("┌───────────────────────────────────────┐")
       if(enemyList[i].state != "Hit" && enemyList[i].state != "Die"){
        if(enemyList[i].FollowTarger == null){ // 유저 찾아오기
            userList.forEach(user => {
                let dx = user.x - enemyList[i].x
                let dy = user.y - enemyList[i].y
                //console.log(`user<->enemy${i} Distance ${Math.abs((user.x - enemyList[i].x) + (user.y - enemyList[i].y))}`);
                if(Math.sqrt(dx * dx + dy * dy) < 5){
                    enemyList[i].FollowTarger = user;          
                }
            });
        }
        
        if(enemyList[i].FollowTarger != null){ // 유저가 비어있지 않으면 그 유저한테로의 상태 설정
            let dx = enemyList[i].FollowTarger.x - enemyList[i].x
            let dy = enemyList[i].FollowTarger.y - enemyList[i].y

            if(Math.sqrt(dx * dx + dy * dy) > 5){
                enemyList[i].FollowTarger = null;
                enemyList[i].state = "MoveAround"
            }
            else if(Math.sqrt(dx * dx + dy * dy) < 1) { 
                enemyList[i].state = "AttackAroundInUser"
            }else if(Math.sqrt(dx * dx + dy * dy) < 5){
                enemyList[i].state = "FollowUser";  
            }

           
        }

        if(enemyList[i].state == "MoveAround"){
            if(enemyList[i].spawnPos.x + 2 < enemyList[i].x || enemyList[i].spawnPos.y + 2 < enemyList[i].y){
                enemyList[i].movePos = -1;
            }
            else if(enemyList[i].spawnPos.x - 2 > enemyList[i].x || enemyList[i].spawnPos.y - 2 > enemyList[i].y){
                enemyList[i].movePos = 1;
            }
            //console.log(`${i} spawnPos : ${enemyList[i].spawnPos.x}  ${enemyList[i].x}  ${enemyList[i].y}  movePos : ${movePos}`)
            
            enemyList[i].x += (Math.random() * enemyList[i].movePos) ;
            enemyList[i].y += (Math.random() * enemyList[i].movePos) ;
        } else if(enemyList[i].state == "FollowUser"){
          
            enemyList[i].x += (enemyList[i].FollowTarger.x - enemyList[i].x) * 0.3
            enemyList[i].y += (enemyList[i].FollowTarger.y - enemyList[i].y) * 0.3 
            
            //console.log(` user<->enemy Distance ${Math.sqrt((dx * dx)+(dy * dy))}`)
            
            for(let j = 0; j < enemyList.length; j++){
                if(i == j){
                    continue;
                }
                let enemyDx = enemyList[i].x - enemyList[j].x;
                let enemyDy = enemyList[i].y - enemyList[j].y;
                //console.log("enemy"+i + "  enemy" + j+ "  Distance " + Math.sqrt((enemyDx * enemyDx) + (enemyDy * enemyDy)));
                if(Math.sqrt((enemyDx * enemyDx) + (enemyDy * enemyDy)) < 0.5){
                    console.log("회피")
                    enemyList[i].x += Math.random() * 2 - 1
                    enemyList[i].y += Math.random() * 2 - 1
                }   
            } 
        }
       }else if (enemyList[i].state == "Hit"){
            enemyList[i].state = "MoveAround";
       }
       
        console.log(`\t${i} EnemyState : ${enemyList[i].state}`)
        if(enemyList[i].FollowTarger != null){
            console.log("\tFollow Target : " + enemyList[i].FollowTarger.id);
        }
        console.log(`\t     EnemyHp : ${enemyList[i].Hp}`)
        
        console.log("└───────────────────────────────────────┘")
    }
    console.log("\n\n\n\n\n\n\n\n")
    
    wss.clients.forEach(function(client) {
        client.send(JSON.stringify({title : "EnemyAround" , enemyList : enemyList}));
    })
}

function EnemyRespawn(){
    enemyList.forEach(enemy => {
        if(enemy.state == "Die"){
            console.log("Respawn Enemy : " + enemy.id);
            enemy.state = "MoveAround";
            enemy.Hp = enemy.MaxHp;
            enemy.x = enemy.spawnPos.x
            enemy.y = enemy.spawnPos.y
        }
    });
    wss.clients.forEach(function each(client) {
        client.send(JSON.stringify({title : "EnemyRespawn" , enemyList : enemyList}));
    })
}

function all_player_response(data){
    wss.clients.forEach(function each(client) {
        client.send(JSON.stringify(data))
    })
}

function without_player_response(data){

    wss.clients.forEach(function each(client) {
        if(data.id != client.id){
            client.send(JSON.stringify(data))
        }
    })
}
function EnemyInit(){
    for(let i = 0; i < 3; i++){
        let x = 5;
        let y = 3 * i + 0.5;
        enemyList.push({
            id : i,
            type : 1,
            x: 5,
            y: 3 * (i + 0.5),
            spawnPos : {x, y},
            FollowTarger : null,
            movePos : 1,
            state : "MoveAround",
            Hp : 100,
            MaxHp : 100,
            AttackTime : 2,
            DropExp : 50
        })
    }

}
function userStateChange(ws){
    for(let i = 0; i < userList.length; i++){
        
        if(userList[i].id == ws.id){
            return i;
        }
    }
    return 'fail'
}
function init(ws){
    return new Promise((resolve ,rejects) => {

        var getUserDataquery = "select user_info.id , hp , mp , strStats , intStats , exp , nick_name , Level from user_status , user_info where user_status.id = ? && user_info.id = ?";
        console.log(id);
        var getUserDataparams = [id,id];
        
        db.query(getUserDataquery , getUserDataparams , function(err , rows , fields){
            userList.push ({
                name : rows[0].id,
                id : rows[0].nick_name,
                x : 0,
                y : 0,
                hp : rows[0].hp,
                mp : rows[0].mp,
                strStats : rows[0].strStats,
                intStats : rows[0].intStats,
                exp : rows[0].exp,
                Level : rows[0].Level,
                maxExp : rows[0].Level * 100
            })
            ws.id =  rows[0].nick_name
            resolve();
        })
        
    })

    
}