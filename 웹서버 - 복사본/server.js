const WebSocket = require('ws')
const express = require("express")
const multer = require('multer');
const path = require('path');
const bodyParser = require("body-parser")
const app = express()

var db = require('./bin/DB.js')


const storage = multer.diskStorage({
    destination: (req, file, cb) => {
        cb(null, 'uploads/');
    },
    filename: (req, file, cb) => {
        cb(null, file.originalname);
    }
});


app.use(bodyParser.json()); 
app.use(bodyParser.urlencoded({ extended: false }));
app.use(express.static("public"))

const uploadDir = path.join(__dirname, 'uploads');
const fs = require('fs');
if (!fs.existsSync(uploadDir)) {
    fs.mkdirSync(uploadDir);
}
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
wss.on('connection', (ws , req) => {


    userId++;
    
    for(let i = 1; i < 100; i++){
        for(let j =0; j < userList.length; j++){
            if(userList[j] == i){
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
    ws.id = userId;
    userList.push(userId);
    console.log(ws.id);

    var data1 = make_data("Init" , userId , userList)
    ws.send(JSON.stringify(data1));

    ws.on('close' , () => {
        for(let i = 0; i < userList.length; i++){
            if(userList[i] == ws.id){
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
            var data2 = make_data("CreateOtherUser" , data.id , userList)
            all_player_response(data2)
        }
        if(data.title == "PlayerMove"){
            data2 = make_data("CheckMove" , data.id , userList , data.x , data.y , data.moveXY , data.state)
            without_player_response(data2 , ws)
        }
        if(data.title == "AttackOtherPlayer"){
            wss.clients.forEach(function each(client) {
                if(client.id != data.id){
                    var data2  = {
                        title : "CheckAttack",
                        id : data.id,
                        x : data.x,
                        y : data.y
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