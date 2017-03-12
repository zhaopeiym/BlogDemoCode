//var Singleton = {
//    name: "农码一生",
//    getName: function () {
//        return this.name;
//    }
//}

//var Person = {
//    age: 27
//}

//var Me = Person;
//Me.name = "农码一生";
//Me.getName = function () {
//    return this.name;
//}
//Me.getAge = function () {
//    return this.age;
//}


//var Singleton = (function () {
//    var name = "农码一生";   
//    return {
//        getName: function () {
//            return name;
//        }
//    }
//})();


//var Singleton = (function () {

//    var Person = function () {
//        this.name = "农码一生";
//    }
//    Person.prototype.getName = function () {
//        return this.name;
//    }
//    var instance;
//    return {
//        getInstance: function () {
//            if (!instance) {
//                instance = new Person();
//            }
//            return instance;
//        }
//    }
//})();

//var person1 = Singleton.getInstance();
//var person2 = Singleton.getInstance();
//console.log(person1 === person2);

////通用的创建单例对象的方法
//var getSingle = function (obj) {
//    var instance;
//    return function () {
//        return instance || (instance = new obj());
//    }
//};

//var PersonA = function () {
//    this.name = "农码一生";
//}

//var PersonB = function () {
//    this.name = "农码爱妹子";
//} 

//var singlePersonA = getSingle(PersonA);//获取PersonA的单例
//var singlePersonB = getSingle(PersonB);//获取PersonB的单例
//var a1 = singlePersonA();
//var a2 = singlePersonA();
//var a3 = singlePersonB();
//var a4 = singlePersonB();
//console.log(a1 === a2);//true
//console.log(a3 === a4);//true
//console.log(a1 === a3);//false 


//通用的创建单例对象的方法
var getSingle = function (obj) {
    var instance;
    return function () {
        return instance || (instance = new obj());
    }
};

//获取tab1的html数据
var getTab1Html = function () {
    this.url = "/tab/tab1.json";
    //$.get(this.url, function (data) {
    //    //这里获取请求到的数据，然后加载到tab页面
    //}, "json");
    console.log("执行");
}

var getTab2Html = function () {
    this.url = "/tab/tab2.json";
    //$.get(this.url, function (data) {
    //    //这里获取请求到的数据，然后加载到tab页面
    //}, "json");
    console.log("执行");
} 

var loadTab1 = getSingle(getTab1Html);
var loadTab2 = getSingle(getTab2Html);

//点击tab1的时候加载tab1的数据
$("#tab1").on("click", function () {
    loadTab1();
})
$("#tab2").on("click", function () {
    loadTab2();
})




