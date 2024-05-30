namespace IntegralCalculator.Model;

public static class Regex
{
    // public static readonly Dictionary<string, string> Operations = new Dictionary<string, string>()
    // {
    //     { @"pow\s*\((.*?),\s*(.*?)\)", "Math.Pow($1, $2)"},
    //     { @"\bln\s*\((.*?)\)", "Math.Log($1)" },
    //     { @"\bsin\s*\((.*?)\)", "Math.Sin($1)" },
    //     { @"\bcos\s*\((.*?)\)", "Math.Cos($1)" },
    //     { @"\btan\s*\((.*?)\)", "Math.Tan($1)" },
    //     { @"\bctg\s*\((.*?)\)", "1/Math.Tan($1)" },
    //     { @"\barcsin\s*\((.*?)\)", "Math.Asin($1)" },
    //     { @"\barccos\s*\((.*?)\)", "Math.Acos($1)" },
    //     { @"\barctg\s*\((.*?)\)", "Math.Atan($1)" },
    //     { @"\barcctg\s*\((.*?)\)", "Math.Atan(1/$1)" },
    //     { @"\bsqrt\s*\((.*?)\)", "Math.Sqrt($1)" },
    //     { @"\bsqrt(\w+)\s*\((.*?)\)", "Math.Pow($2, 1/$1)" },
    //     { @"\bpi\b", "Math.PI" },
    //     { @"\be\b", "Math.E" }
    // };
    
    public static readonly Dictionary<string, string> Operations = new Dictionary<string, string>()
    {
        { @"\bpi\b", "PI" },
        { @"\be\b", "E" },
        { @"pow\s*\((.*?),\s*(.*?)\)", "Pow($1, $2)"},
        { @"(\d+)((sin|cos|tg|ctg|lg|log|ln|sqrt|sqrt\d+|pow|arcsin|arccos|arcctg|arctg)\(.+\)|[a-z]\b)", "$1 * $2" },
        { @"((sin|cos|tg|ctg|lg|log|ln|sqrt|sqrt\d+|pow|arcsin|arccos|arcctg|arctg)\(.+\)|\b[a-z])(\d+)", "$3 * $1" },
        { @"\blg\s*\((.*?)\)", "Log10($1)" },
        { @"\bln\s*\((.*?)\)", "Ln($1)" },
        { @"\blog\s*\((.*?),\s*(.*?)\)", "Log($1, $2)" },
        { @"\bsin\s*\((.*?)\)", "Sin($1)" },
        { @"\bcos\s*\((.*?)\)", "Cos($1)" },
        { @"\btg\s*\((.*?)\)", "Tan($1)" },
        { @"\bctg\s*\((.*?)\)", "1/Tan($1)" },
        { @"\barcsin\s*\((.*?)\)", "Asin($1)" },
        { @"\barccos\s*\((.*?)\)", "Acos($1)" },
        { @"\barctg\s*\((.*?)\)", "Atan($1)" },
        { @"\barcctg\s*\((.*?)\)", "Tan(1/$1)" },
        { @"\bsqrt\s*\((.*?)\)", "Sqrt($1)" },
        { @"\bsqrt(\w+)\s*(\(((?:[^)(]+|(?2))*+)\))", "Pow($3, 1/$1)" },
        
    };
    
    public static readonly Dictionary<string, string> Markdowns = new Dictionary<string, string>()
    {
        { @"((sin|cos|tg|ctg|lg|log|ln|sqrt|sqrt\d+|pow|arcsin|arccos|arcctg|arctg)\(.+\)|\b[a-z])(\d+)", "$3*$1" },
        { @"(\d+)((sin|cos|tg|ctg|lg|log|ln|sqrt|sqrt\d+|pow|arcsin|arccos|arcctg|arctg)\(.+\)|[a-z]\b)", "$1 * $2" },
        { @"(\(.+\)|\w+\(.+\)|\w+)\/(\(.+\)|\w+\(.+\)|\w+)", @"\frac{$1}{$2}"},
        { @"\bpow\s*(\(((?>[^)(,]+|(?1))*)(?>\s*,\s*((?>[^)(,]+|(?1))*))*\))", "{$2}^{$3}"},
        { @"\bsqrt\s*(\(((?:[^)(]+|(?1))*+)\))", @"\sqrt{$2}" },
        { @"\bsqrt(\w+)\s*(\(((?:[^)(]+|(?2))*+)\))", @"\sqrt[$1]{$3}" },
        { @"\\frac{\((.+)\)}{(.+)}", @"\frac{$1}{$2}"},
        { @"\\frac{(.+)}{\((.+)\)}", @"\frac{$1}{$2}"},
        { @"\b(?<!\\)pi\b", @"{\pi}" },
        { @"\b(?<!\\)lg\s*(\(((?:[^)(]+|(?1))*+)\))", @"\lg($2)" },
        { @"\b(?<!\\)ln\s*(\(((?:[^)(]+|(?1))*+)\))", @"\ln($2)" },
        { @"\b(?<!\\)sin\s*(\(((?:[^)(]+|(?1))*+)\))", @"\sin($2)" },
        { @"\b(?<!\\)cos\s*(\(((?:[^)(]+|(?1))*+)\))", @"\cos($2)" },
        { @"\b(?<!\\)tg\s*(\(((?:[^)(]+|(?1))*+)\))", @"\tg($2)" },
        { @"\b(?<!\\)ctg\s*(\(((?:[^)(]+|(?1))*+)\))", @"\ctg($2)" },
        { @"\b(?<!\\)arcsin\s*(\(((?:[^)(]+|(?1))*+)\))", @"\arcsin($2)" },
        { @"\b(?<!\\)arccos\s*(\(((?:[^)(]+|(?1))*+)\))", @"\arccos($2)" },
        { @"\b(?<!\\)arctg\s*(\(((?:[^)(]+|(?1))*+)\))", @"\arctg($2)" },
        { @"\b(?<!\\)arcctg\s*(\(((?:[^)(]+|(?1))*+)\))", @"arcctg($2)" },
        { @"\b(?<!\\)log\s*(\(((?>[^)(,]+|(?1))*)(?>\s*,\s*((?>[^)(,]+|(?1))*))*\))", @"\log_{$2}{$3}" }
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