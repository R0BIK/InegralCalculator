namespace IntegralCalculator.Model;

public class Dictionary
{
    public static Dictionary<string, string> operations = new Dictionary<string, string>()
    {
        { @"pow\s*\((.*?),\s*(.*?)\)", "Math.Pow($1, $2)"},
        { @"\bln\s*\((.*?)\)", "Math.Log($1)" },
        { @"\bsin\s*\((.*?)\)", "Math.Sin($1)" },
        { @"\bcos\s*\((.*?)\)", "Math.Cos($1)" },
        { @"\btan\s*\((.*?)\)", "Math.Tan($1)" },
        { @"\bctg\s*\((.*?)\)", "1/Math.Tan($1)" },
        { @"\barcsin\s*\((.*?)\)", "Math.Asin($1)" },
        { @"\barccos\s*\((.*?)\)", "Math.Acos($1)" },
        { @"\barctg\s*\((.*?)\)", "Math.Atan($1)" },
        { @"\barcctg\s*\((.*?)\)", "Math.Atan(1/$1)" },
        { @"\bsqrt\s*\((.*?)\)", "Math.Sqrt($1)" }
    };
    
    public static Dictionary<string, string> markdowns = new Dictionary<string, string>()
    {
        { @"(\(.+\)|\w+\(.+\)|\w+)\/(\(.+\)|\w+\(.+\)|\w+)", @"\frac{$1}{$2}"},
        { @"pow\s*\((.*?),\s*(.*?)\)", "{$1}^{$2}"},
        { @"\bsqrt\s*\((.*?)\)", @"\sqrt{$1}" }
    };
    
    // { @"\bsin\s*\((.*?)\)", @"\sin($1)" },
    // { @"\bcos\s*\((.*?)\)", @"\cos($1)" },
    // { @"\btan\s*\((.*?)\)", @"\tan($1)" },
    // { @"\bctg\s*\((.*?)\)", @"\frac{1}{\tan($1)}" },
    // { @"\barcsin\s*\((.*?)\)", @"\arcsin($1)" },
    // { @"\barccos\s*\((.*?)\)", @"\arccos($1)" },
    // { @"\barctg\s*\((.*?)\)", @"\arctan($1)" },
    
    // public Dictionary<string, string> operations = new Dictionary<string, string>()
    // {
    //     { @"\bln\b", "Math.Log" },
    //     { @"\bsin\b", "Math.Sin" },
    //     { @"\bcos\b", "Math.Cos" },
    //     { @"\btan\b", "Math.Tan" },
    //     { @"\bctg\b", "1/Math.Tan" },
    //     { @"\barcsin\b", "Math.Asin" },
    //     { @"\barccos\b", "Math.Acos" },
    //     { @"\barctg\b", "Math.Atan" },
    //     { @"\bsqrt\b", "Math.Pow" }, //!!!
    //     { @"\barcctg\b", "Math.Sqrt" },
    //     { @"\barcctg\b", "Math.Atan" },
    //     { @"\barcctg\b", "Math.Atan" },
    //     { @"\barcctg\b", "Math.Atan" },
    // };
}