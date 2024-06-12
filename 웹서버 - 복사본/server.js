const WebSocket = require('ws')
const express = require("express")
const path = require('path');
const bodyParser = require("body-parser")
const app = express()

var db = require('./bin/DB.js');
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
            console.log(userList);
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
        if(data.title == "EnemyPos"){
            console.log(data.id + " " + data.state);
            var EnemyPos = {
                title : "enemyMove",
                id : data.id,
                x : data.x,
                y : data.y,
                state : data.state
            }
            wss.clients.forEach(function each(client){
                client.send(JSON.stringify(data2))
            })
        }

})
})

wss.on('listening' , () => {
    console.log('리스닝')
})

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
        enemyList.push({
            id : i,
            type : 1,
            x: 5,
            y: 3 * (i + 0.5)
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