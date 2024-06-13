const WebSocket = require('ws')
const express = require("express")
const path = require('path');
const bodyParser = require("body-parser")
const app = express()

//var db = require('./bin/DB.js');
const { debug } = require('console');

app.use(bodyParser.json()); 
app.use(bodyParser.urlencoded({ extended: false }));
app.use(express.static("public"))

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
    console.log(reslut.NeedMapName);
    var query = 'SELECT * FROM MAPDATA WHERE MAPNAME = ?';
    var parmas = reslut.NeedMapName;
    db.query(query , parmas , function(err , rows , fields) {
        var data = {
            "mapData" : rows
        }
        res.send(data);
    })
})

const wss = new WebSocket.Server({port : 8000}, () => {
    console.log('wss server port 8000')
})
var userId = 0
var enemyList = []
var userList = []
EnemyInit()

wss.on('connection', (ws , req) => {
    init();
    ws.id = userId;

    var data1 = JSON.stringify({
        title : "Init" ,
        id : userId ,
        users : userList ,
        enemyList : enemyList
    });
    
    console.log(data1);
    ws.send(data1);

    ws.on('close' , () => {
        for(let i = 0; i < userList.length; i++){
            if(userList[i].id == ws.id){
                userList.splice(i,1);
            }
        }
        console.log(userList)
        var data2 = {
            title : "CheckOutUser" ,
            users : userList,
            id : ws.id
        }
        all_player_response(data2)
    })
    ws.on('message', function message(data) {
        
        data = JSON.parse(data)

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
        if(data.title == "AttackOtherPlayer"){
            wss.clients.forEach(function each(client) {
                if(client.id != ws.id){
                    var data2  = {
                        title : "CheckAttack",
                        id : ws.id
                    }
                    client.send(JSON.stringify(data2))
                }
            })
        }
})
})

wss.on('listening' , () => {
    console.log('리스닝')
})

setInterval(EnemyChangeState , 1000)
let movePos = [1 , 1 , 1]; // 생성할 Enemy 의 갯수만큼 만들어줘야됌
function EnemyChangeState(){
    
    console.log("┌───────────────────────────────────────┐")
    for(let i = 0; i < enemyList.length; i++){

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

        //console.log(enemyList)

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
        } else {
            let dx = enemyList[i].FollowTarger.x - enemyList[i].x
            let dy = enemyList[i].FollowTarger.y - enemyList[i].y
            if(enemyList[i].state == "FollowUser"){
                enemyList[i].x += (enemyList[i].FollowTarger.x - enemyList[i].x) * 0.3
                enemyList[i].y += (enemyList[i].FollowTarger.y - enemyList[i].y) * 0.3 
            }
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
        
        console.log(`\t${i} EnemyState : ${enemyList[i].state}`)
        
    }
    console.log("└───────────────────────────────────────┘")

    wss.clients.forEach(function(client) {
        client.send(JSON.stringify({title : "EnemyAround" , enemyList : enemyList}));
    })
}

function make_data(title , id , userList , x , y , moveXY , state){
    var data = {title : title,
        id : id,
        users : userList ,
        x : x,
        y : y,
        moveXY : moveXY,
        state : state
    }
    return data
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
            state : "MoveAround"
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
function init(){

    userId++;
    
    for(let i = 1; i < 100; i++){
        for(let j = 0; j < userList.length; j++){
            if(userList[j].id == i){
                userId = 0;
                break;
            }
            else {
                userId = i;
            }
        }
        if(userId != 0){
            break;
        }
    }
    console.log(userId);
    userList.push({id : userId ,
         x : 0 ,
         y : 0});

    
}