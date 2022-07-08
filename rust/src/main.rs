extern crate pest;
#[macro_use]
extern crate pest_derive;

use pest::Parser;
use pest::iterators::Pairs;

#[derive(Parser)]
#[grammar = "avx.pest"]
struct QuelleParser;

fn main() {
    let tests = QuelleParser::parse(Rule::segment, "he \t said").unwrap_or_else(|e| panic!("{}", e));
    output(tests, 0);

    let pairs = QuelleParser::parse(Rule::command, "\"#foo ... [he \t said] ... /pronoun/&#3\" + bar + x|y&z a&b&c > xfile < genesis 1:1").unwrap_or_else(|e| panic!("{}", e));
    output(pairs, 0);
}

fn indent(tabs: u32) {
    for i in 0 .. tabs {
        print!("\t");
    }
}

fn output(children: Pairs<Rule>, tabs: u32)
{
    for pair in children {
///////THIS IS NOW HANDLED BY PEG GRAMMAR///////////////////////////////////////////////////////////////////
//     if pair.as_rule() == Rule::alpha || pair.as_rule() == Rule::digit || pair.as_rule() == Rule::legal {
//         break;
//     }

        // A pair is a combination of the rule which matched and a span of input
        indent(tabs);
        println!("Rule:    {:?}", pair.as_rule());
        indent(tabs);
        println!("Text:    {}", pair.as_str());
        indent(tabs);
        println!("Span:    {:?}", pair.as_span());

        // A non-terminal pair can be converted to an iterator of the tokens which make it up:
        output(pair.into_inner(), tabs+1);
    }
}