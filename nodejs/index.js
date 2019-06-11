var passport = require("passport");
var bodyParser = require("body-parser");
var pg = require('pg');
var cool = require('cool-ascii-faces');
var express = require('express');
var app = express();

var loggedin = 0;

app.set('port', (process.env.PORT || 5000));

app.use(express.static(__dirname + '/public'));
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());

// views is directory for all template files
app.set('views', __dirname + '/views');
app.set('view engine', 'ejs');

app.get('/', function(request, response) {
  response.render('pages/index');
});

app.get('/cool', function(request, response) {
  response.send(cool());
});

/*
app.post('/login',
  passport.authenticate('local', { successRedirect: '/',
                                   failureRedirect: '/login' }));
*/

app.get('/login', function(request, response) {
  response.render('pages/login');
});

app.post('/login', function(request, response){
    var user = request.body.username;
    var psw = request.body.password;
    
    if(user.length > 1 && psw.length > 1){
        pg.connect(process.env.DATABASE_URL, function(err, client, done) {
            if(!err){
                client.query("select id from users where username = '" + user + "' and password = '" + psw + "'" , function(err, result) {
                  done();
                  if (err)
                   { console.error(err); response.send("Error " + err); }
                  else
                   { 
                     if(result != null && result.rowCount != 0){ 
                        loggedin = result.rowCount;
                        response.render('pages/weight', {users: result.rowCount} );
                     }else{ 
                        response.render('pages/login', {results: 'error: username or password incorrect.'} ); 
                     }  
                   }
                });
            }else{ console.error(err);}
        });
    }else{
        response.render('pages/login', {results: 'error: username and password cannot be empty.'} );
    }
      
});

app.get('/register', function(request, response) {
  response.render('pages/register');
});

app.post('/register', function(request, response){
    var user = request.body.username;
    var psw = request.body.password;

    if ((user.length > 1 && psw.length > 1)){
        pg.connect(process.env.DATABASE_URL, function(err, client, done) {
            if(!err){
                client.query("insert into users values(DEFAULT, '" + user + "', '" + psw + "')" , function(err, result) {
                    done();
                    if (err)
                    { console.error(err); response.render('pages/register', {results: "Error " + err}); }
                    else
                    { 
                        if(result != null){ 
                            console.log(result.rowCount); 
                            response.render('pages/register', {results: result.rowCount} );
                        }else{ 
                            response.render('pages/register', {results: 'error: the user could not be registered.'} ); 
                        }  
                    }
                });
            }else{ console.error(err);}
        });
    }else{
        response.render('pages/register', {results: 'error: username and password cannot be empty.'} );
    }
});

app.get('/weight', function(request, response) {
    if(loggedin != 0){
        response.render('pages/weight');
    }else{ response.render('pages/login'); }
});

app.post('/weight', function(request, response){
    var weight = request.body.weight;
    if (weight.length > 1){
        pg.connect(process.env.DATABASE_URL, function(err, client, done) {
            if(!err){
                client.query("select * from weights where from_bmi <= " + weight + " and to_bmi >= " + weight + "" , function(err, result) {
                    done();
                    if (err)
                    { response.render('pages/weight', {results: "Error " + err}); }
                    else
                    { 
                        if(result != null){ 
                            console.log(result.rowCount); 
                            response.render('pages/weight', {results: result.rows} );
                        }  
                    }
                });
            }else{ console.error(err);}
        });
    }
});

app.get('/times', function(request, response) {
    var result = ''
    var times = process.env.TIMES || 5
    for (i=0; i < times; i++)
      result += i + ' ';
  response.send(result);
});

app.get('/db', function (request, response) {
  console.log(process.env.DATABASE_URL);
  pg.connect(process.env.DATABASE_URL, function(err, client, done) {
    if(!err){
        client.query('select * from test_table', function(err, result) {
          done();
          if (err)
           { console.error(err); response.send("Error " + err); }
          else
           { if(result != null){ console.log(result); response.render('pages/db', {results: result.rows} ); }  }
        });
    }else{ console.error(err);}
  });
});

app.listen(app.get('port'), function() {
  console.log('Node app is running on port', app.get('port'));
});


