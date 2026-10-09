using Backend;

var infix = "4*5/(4+6)";
Console.WriteLine($"Infix = {infix}, Result = {ExpressionEvaluator.Evalute(infix):N5}"); 

var infix2 = "4*(5+6-(8/2^3)-7)-1";
Console.WriteLine($"Infix = {infix2}, Result = {ExpressionEvaluator.Evalute(infix2):N5}"); 

var infix3 = "4*7^(1/3)*7*((1+9)/3*7^4)";
Console.WriteLine($"Infix = {infix3}, Result = {ExpressionEvaluator.Evalute(infix3):N5}"); 

var infix4 = "144^(1/2)";
Console.WriteLine($"Infix = {infix4}, Result = {ExpressionEvaluator.Evalute(infix4):N5}"); 




