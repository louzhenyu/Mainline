// value为大于0的正整数时，返回true；否则，返回false：
function regLargeThanZeroInt(value)
{  
    var reg = /^[1-9]\d*$/;
    return reg.test(value); // value是待验证的值
}