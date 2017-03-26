
//鸭子类型===================
var 鸭子 = {
    走路: function () {
        console.log("走路咯");
    },
    咕咕咕: function () { }
}

var 鸟 = {
    走路: function () {
        console.log("走路咯");
    },
    咕咕咕: function () { }
}

var 鸭子们 = [];
鸭子们.push(鸭子);
鸭子们.push(鸟);

for (var i = 0; i < 鸭子们.length; i++) {
    鸭子们[i].走路();
}



//封装=================
var Person = (function () {
    var sex = "纯爷们";
    return {
        name: "农码一生",
        getInfo: function () {
            console.log("name:" + this.name + ",sex:" + sex);
        }
    };
})();



//继承======================
var Person = {
    name: "农码一生",
    getName: function () {
        console.log(this.name);
    }
};
var obj = Person;
obj.getName();




var Person = function () {
    this.name = "农码一生";
}
Person.prototype.getName = function () {
    console.log(this.name);
}

var obj = function () { };
obj.prototype = new Person();//obj继承于Person

var o = new obj();
o.getName();//直接调用原形中的getName（类似于C#中的调用父类方法）


//原型====================
//1
var Person = function () {
    this.name = "农码一生";
    this.sex = "纯爷们";
};
console.log(Person.prototype);

//2
var Person = function () {
    this.name = "农码一生";
    this.sex = "纯爷们";
    this.getInfo = function () {
        console.log("name:" + this.name + ",sex:" + this.sex);
    }
};
var p = new Person();
var p1 = new Person();
console.log(p.getInfo === p1.getInfo);

//3
var Person = function () {
    this.name = "农码一生";
    this.sex = "纯爷们";
};
Person.prototype.getInfo = function () {
    console.log("name:" + this.name + ",sex:" + this.sex);
};
var p = new Person();
var p1 = new Person();
console.log(p.getInfo === p1.getInfo);
//4
var Person = function () {
    this.name = "农码一生";
    this.sex = "纯爷们";
    this.getInfo = function () {
        console.log("name:" + this.name + ",sex:" + this.sex);
    }
};
var Student = function () { };
Student.prototype = new Person();//继承
var s1 = new Student();
var s2 = new Student();
console.log(s1.getInfo === s2.getInfo);
console.log(s1.getInfo === s2.getInfo);
//5
var Person = function () {
    this.name = "农码一生";
};
var Student = function () { };
Student.prototype = new Person();//继承
Student.prototype.constructor = Student;
var stu = new Student();

//Student.prototype.getName = function () {
//    console.log("我的名字:" + this.name);
//}
Person.prototype.getName = function () {
    console.log("My name is:" + this.name);
}

stu.getName();

//6

//this===================

//1
window.name = "张三";
var obj = {
    name: "李四",
    getName: function () {
        console.log(this.name);
    }
}
//obj.getName();
window.func = obj.getName;
window.func();

//2
window.name = "张三";
var obj = {
    name: "李四",
    getName: function () {
        console.log(this.name);
    }
}
//obj.getName();
window.func = obj.getName;
window.func.call(obj);


//3
function func() {
    console.log("我点击了" + $(this).find("option:selected").text());
}

$("#element").change(function () {
    func.call(this);
});


//4
function func(age, sex) {
    console.log("name:" + this.name + ",age:" + age + ",sex:" + sex);
}

var obj = {
    name: "晓梅"
}

//func.call(obj, "18", "妹子");
//func.apply(obj,["18","小美女"]);
var func1 = func.bind(obj, "18", "妹子");
func1();

//闭包============================
var Person = (function () {
    var sex = "纯爷们";
    return {
        name: "农码一生",
        getInfo: function () {
            console.log("name:" + this.name + ",sex:" + sex);
        }
    };
})();

//2

for (var i = 0; i < 10; i++) {
    var t = setTimeout(function () {
        console.log(i);
    }, 100);
}

//3
for (var i = 0; i < 10; i++) {
    (function (i) {
        var t = setTimeout(function () {
            console.log(i);
        }, 100);
    })(i);
}

//4

var getInfo = function (callback) {
    $.ajax('url', function (data) {
        if (typeof callback === 'function') {
            callback(data);
        }
    });
}
getInfo(function (data) {
    alert(data.userName);
});







//原型继
//JavaScript 中的根对象是 Object.prototype 对象,Object.prototype 对象是一个空的对象
//当使用 new 运算符来调用函数时，此时的函数就是一个构造器。
//用new 运算符来创建对象的过程，实际上也只是先克隆 Object.prototype 对象，再进行一些其他额外操作的过程。
//将“不变的事物”与 “可能改变的事物”分离开来