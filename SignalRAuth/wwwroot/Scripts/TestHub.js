const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost/SignalRAuth/test")
    .configureLogging(signalR.LogLevel.Trace)
    .build();
connection.start().catch(function (err) { console.error(err.toString()); });

connection.on('send', function (username) {
    console.log('connected: ' + username);
    $('#connectStatus').text(username).change();
});