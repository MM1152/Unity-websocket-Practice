const WebSocket = require('ws')

const wss = new WebSocket.Server({port : 8000}, () => {
    console.log('서버 시작')
})
var userId = 0
var enemyList = []
var userList = []
wss.on('connection', (ws , req) => {

    let x = 0;
    let y = 0;

    userId++;
    userList.push(userId)
    
    ws.id = userId;
    console.log(ws.id);

    var data1 = make_data("Init" , userId , userList)
    ws.send(JSON.stringify(data1));

    ws.on('close' , () => {
        for(let i = 0; i < userList.length; i++){
            if(userList[i].userId == ws.id){
                userList.splice(i,1);
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
        
        if(data.title == "EndConnection"){
            console.log(`disconnetion wsid : ${ws.id}`);
 
            for(let i = 0; i < userList.length; i++){
                if(userList[i] == data.id){
                    userList.splice(i,1);
                }
            }
           
            wss.clients.forEach(function each(client) {           
                if (client.readyState === WebSocket.OPEN) {
                    var data2 = {
                        title : "checkOutUser" ,
                        users : userList ,
                        id : data.id
                    } 
                    client.send(JSON.stringify(data2));
                
                }
        })
        }
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

function without_player_response(data , ws){
    wss.clients.forEach(function each(client) {
        if(ws.id != client.id){
            client.send(JSON.stringify(data))
        }
    })
}