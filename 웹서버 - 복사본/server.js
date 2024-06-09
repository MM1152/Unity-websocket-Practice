const WebSocket = require('ws')
const requestIp = require('request-ip')

const wss = new WebSocket.Server({port : 8000}, () => {
    console.log('서버 시작')
})
var userId = 0
var userList = []
wss.on('connection', (ws , req) => {

    
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
    var data1 = {
        title : "checkUserID",
        id : userId,
        users : userList
    }
    
    ws.send(JSON.stringify(data1));
    
    ws.on('message', function message(data) {
        data = JSON.parse(data)
   
        if(data.title == "EndConnection"){
            console.log("send EndConnectionMessage");
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
            console.log("send ConntionMesssage");
                    
            wss.clients.forEach(function each(client) {       
                if (client.readyState === WebSocket.OPEN) {
                    var data2 = {
                        id : data.id ,
                        title : "checkCreateUser" ,
                        users : userList
                    } 
                    client.send(JSON.stringify(data2));        
                }
            })
        }
        if(data.title == "PlayerMove"){
            wss.clients.forEach(function each(client) {
                if(client != ws && client.readyState === WebSocket.OPEN){
                    var data2 = {
                        title : "checkMove",
                        id : data.id ,
                        x : data.x ,
                        y : data.y, 
                        moveXY : data.moveXY
                    }
                    client.send(JSON.stringify(data2));
            } 
        })
        }
        if(data.title == "AttackOtherPlayer"){
            console.log(data.moveXY)
            wss.clients.forEach(function each(client) {
                if(client.id != data.id){
                    var data2  = {
                        title : "checkAttack",
                        id : data.id,
                        x : data.x,
                        y : data.y
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