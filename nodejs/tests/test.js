var random = require('random');
var assert = require('assert');

describe('login', function(){
  it('should respond to GET',function(){
    superagent
      .get('http://localhost:5000/login')
      .end(function(res){
        expect(res.status).to.equal(200);
    })
});

describe('register', function(){
  it('should respond to GET',function(){
    superagent
      .get('http://localhost:5000/register')
      .end(function(res){
        expect(res.status).to.equal(200);
    })
});

describe('register', function(){
  it('should respond to POST and Return Error',function(){
    superagent
      .post('http://localhost:5000/register')
      .send({"username":"","password":""})
      .end(function(res){
        expect(res.results).to.equal("error: username and password cannot be empty.");
    })
});

describe('register', function(){
  it('should respond to POST and Return Succesfull',function(){
    superagent
      .post('http://localhost:5000/register')
      .send({"username":random.generate(7),"password":random.generate(7)})
      .end(function(res){
        expect(res.results).to.equal(1);
    })
});

describe('weight', function(){
  it('should respond to GET',function(){
    superagent
      .get('http://localhost:5000/weight')
      .end(function(res){
        expect(res.status).to.equal(200);
    })
});

describe('weight', function(){
  it('should respond to POST and Return a Result String',function(){
    superagent
      .post('http://localhost:5000/weight')
      .send({"weight":random.generate(numeric)})
      .end(function(res){
        expect(res.results).to.be.an('object');
    })
});

describe('login', function(){
  it('should respond to POST and Return 1',function(){
    superagent
      .post('http://localhost:5000/login')
      .send({"username":"demo@demo.com","password":"demo"})
      .end(function(res){
        expect(res.users).to.equal(1);
    })
});

describe('login', function(){
  it('should respond to POST and Return Error',function(){
    superagent
      .post('http://localhost:5000/login')
      .send({"username":"zzz","password":"zzz"})
      .end(function(res){
        expect(res.results).to.equal("error: username or password incorrect.");
    })
});

describe('login', function(){
  it('should respond to POST and Return Error',function(){
    superagent
      .post('http://localhost:5000/login')
      .send({"username":"","password":""})
      .end(function(res){
        expect(res.results).to.equal("error: username and password cannot be empty.");
    })
});