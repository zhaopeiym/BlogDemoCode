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




