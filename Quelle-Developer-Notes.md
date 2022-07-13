# Quelle Developer Notes

##### version 1.0.2707 (not fully updated yet to the 1.02xxx version)

1. SEARCH
   - *find*
2. CONTROL
   - *set*
   - *clear*
   - @get
3. DISPLAY
   - *show*
   - *output*
4. SYSTEM
   - @help
   - @version
   - @exit
5. HISTORY
   - @review
   - *invoke*
6. LABEL
   - *save*
   - @delete
   - @expand
   - *execute*

| OUT-OF-DATE<br>Verb | Syntax Category | Required Parameters     | Required Operators | Optional Operators | Provider or Driver |
| ------------------- | --------------- | ----------------------- | :----------------: | :----------------: | ------------------ |
| *find*              | SEARCH          | **1**: *search spec*    |                    |  **" " [ ] ( )**   | provider           |
| **@status**         | SEARCH          | 0 or 1                  |                    |                    | provider           |
| *set*               | CONTROL         | **2**: *name* = *value* |       **=**        |                    | driver             |
| *clear*             | CONTROL         | **1**: *control_name*   |       **=@**       |                    | driver             |
| **@get**            | CONTROL         | **0+**: *control_names* |                    |                    | driver             |
| **@print**          | DISPLAY         | **0+**: *identifiers*   |                    |   **[ ]  !  >**    | provider           |
| **@save**           | LABEL           | **1**: *macro_label*    |                    |                    | driver             |
| **@delete**         | LABEL           | **1+**: *macro_labels*  |      **{ }**       |                    | driver             |
| **@show**           | LABEL           | **0+**: *macro_labels*  |                    |      **{ }**       | driver             |
| *execute*           | LABEL           | 1                       |      **{ }**       |                    | driver             |
| **@review**         | HISTORY         | 0 or 1                  |                    |                    | driver             |
| *invoke*            | HISTORY         | 1                       |      **{ }**       |                    | driver             |
| **@help**           | SYSTEM          | 0 or 1                  |                    |                    | driver             |
| **@generate**       | SYSTEM          | 2 or 4                  |                    |      **!  >**      | driver (hidden)    |
| **@exit**           | SYSTEM          | 0                       |                    |                    | driver             |

One command not listed in the users guide is @generate. While not a normal user function, it can be useful for Quelle Search Provider developers as it generates code for interop with the standard C# reference implementation.

*@generate* system command assists <u>programmers and developers</u>

indentation=tab

indentation=spaces:8

There are two required parameters for the @generate command: the programming-language and the name of the class.  Consult the source-code on GitHub for classnames.  Or just code-generate CloudSearch first and find dependent imports in that code-generated class, to determine additional code-generation requirements.

*@generate* Java IQuelleSearchRequest

The generate command will generate the internal Quelle class in the language specified. Indentation will be controlled as specified by a separate CONTROL statement.  Quelle's communication with a web-search provider [aka host] uses an HTTPS POST request and JSON serialization of C# classes that contain the parsed Quelle clauses.  Generating these classes accelerates the development of deserializers for the language of the search host.  In each invocation, the class/structure is code-generated into the language specified.  Languages & IDL supported are:

- Java
- Go
- C
- C#
- Rust
- Protobuf
- gRPC

In the case of gRPC, the third parameter must be "*" as it always generates all messages, in addition to the Quelle cloud-service definitions.

The additional two parameters are optional, and are also very specific.  If the third parameter is provided, it must be the greater-than symbol ( > ).  And the final and fourth parameter must be a valid path+filename specification. To expand on the previous example, we can save output to a file with this command:

*@generate* Java IQuelleSearchRequest >  C:\\MyFolder\\src\\IQuelleSearchRequest.java

The folder must exist, and the file in that folder must not exist.  If those two conditions are met, the CloudSearch.java will contain the generated code.

If the user does not care if the file already exists, the existence check can be bypassed by adding exclamation ( ! ) to the command:

*@generate**!*** Java IQuelleSearchRequest >  C:\\MyFolder\\src\\IQuelleSearchRequest.java

Finally, to generate IDL for all cloud-interface types, issue this command:

*@generate* gRPC * >  C:\\MyFolder\\src\\QuelleCloudSearchProvider.proto

NOTE: To be clear, the standard Quelle driver does <u>not</u> utilize gRPC or Protocol Buffers.  Yet, the IDL is useful and the driver could be extended by other developers.  The Standard Quelle driver is implemented in DotNet 5.0 and C#.  The reference implementation of a Quelle Search Provider is REST service implemented in Rust.

| Long Name          | Short Name  | Meaning                                                     | Values                                 | Passed to search provider | Notes                             |
| ------------------ | ----------- | ----------------------------------------------------------- | -------------------------------------- | ------------------------- | --------------------------------- |
| search.span        | span        | proximity                                                   | 0 to 1000                              | yes                       |                                   |
| search.domain      | domain      | search domain                                               | string                                 | yes                       |                                   |
| search.exact       | exact       | exact vs liberal/fuzzy                                      | true/false                             | yes                       |                                   |
| display.heading    | heading     | heading of results                                          | string                                 | no                        |                                   |
| display.record     | record      | fetch result annotations                                    | string                                 | no                        |                                   |
| display.format     | format      | page result format                                          | Table 9-2                              | yes                       |                                   |
| system.indentation | indentation | specifies tabs or spaces on when invoking @generate command | tab, spaces:2, spaces:3, spaces:4, ... | no                        | hidden                            |

Macros are yaml files which include values for all controls.  In the example below, the yaml file would be *named my-macro-label.yaml*.  Macros are always case-insensitive.  And hyphens and spaces are synonymous when naming the macro.

```yaml
search:
​	span: 7
​	domain: kjv
​	extact: false
display:
​	heading: !!null
​	record: !!null
#	format: html    # need not be included, because it has no effect on macros
#	output: !!null	# need not be included, because it has no effect on macros
system:
#	indentation: 4  # need not be included, because it has no effect on macros
definition:
​	label:	My Macro Label
​	macro:	eternal power godhead ; Jehova
```

Control variable share this common format.  However, they are split across three distinct files and occur without the yaml section headings.



search.yaml

```yaml
span: 7
domain: kjv
extact: false
```



display.yaml

```yaml
heading: !!null
record: !!null
format: html
```



system.yaml

```yaml
indentation: 0
```



Terse & concise parsing notes (these test-cases need to be included in unit-tests):

```
a b c|d // "e|f g h|i j" // "k|l...m...n|o" // "[p|q r s] t" //"*men boy*"
clause 1: (unquoted)		/ NEGATIVE
	segment 1.1:	 a 	/ position = none / SINGLETON
	segment 1.2:	 b 	/ position = none / SINGLETON
	segment 1.3:	 c|d 	/ position = none / SET  (segment 1.3 has two fragments)
clause 2: (quoted)
	segment 2.1:	 e|f 	/ t1: position = 1 / SET
	segment 2.2:	 g 	/ position = t1+1 / SINGLETON
	segment 2.3:	 h 	/ g+1 >= position <= g+2 / SINGLETON
	segment 2.3:	 i 	/ g+1 >= position <= g+2 / SINGLETON
	segment 2.4:	 j 	/ position = g + 3 / SINGLETON
clause 3: (quoted)
	segment 3.1:	 k 	/ 1 >= position <= 2 / SINGLETON
	segment 3.2:	 l 	/ 1 >= position <= 2 / SINGLETON
	segment 3.3:	 m 	/ t3: 3 <= position <= span/ SINGLETON
	segment 3.4:	 n|o 	/ t3 > position <= span / SET
clause 4: (quoted)
	segment 4.1:	 p|q	/ 1 >= position <= 3 / SET
	segment 4.2:	 r 	/ 1 >= position <= 3 / SINGLETON
	segment 4.3:	 s 	/ 1 >= position <= 3 / SINGLETON
	segment 4.4:	 t 	/ position = 4 /SINGLETON
clause 5: quoted
	segment 5.1:	(men women) 		 / position = 1 / SET
	segment 5.2:	(boy boys boycott) 	 / position = 2 / SET


We will also add the & symbol to allow matching upon a single token (like PN and stem).
For parsing purposes spaces WILL NOT be allowed around the &. Example:
/pronoun#2PS/&/BOV/ #run&/v/
^^^
(This clause has two segments; both segments have one fragment, even though the anded expression contains two tokens)
By restricting the tokenfgroup above, we do not need specialized parsing for AVX, the
execute method will desipher meaning of tokens.

Unrelated to any of this, we shouild be able to set the span to "Verse Scope".
We could represent this with a span = 0.

STEP 1
======
break command into macro (left-side) and clause-array

The first byte is the type:
 AV | AVX | AV+AVX | AV != AVX (diff) | SYM | Eo? | Lemma(AV) | Lemma(AVX) | Part-Of-Speech | WordClass | Person-Number
If Specialized, then the next 3 bytes are used to identify the the type of symbol (zero-bits means any symbol for \SYM\)
If AV and/or AVX, the the last 16-bit word is the word-key


STEP 2
======
for each command
parse using new peg-based parser

ADD specialized fragments to Quelle 1.0.2
========================================
! (reve)
/SYMbol/	(any non-punctuation symbol / i.e non-alpha-numeric)
/PUNCtuation/	(any punctuation / i.e non-alpha-numeric)
/,./	(comma or period)
/EoV/	(end of verse)
/BoV/	(beginning of verse)
/EoC/	(end of chapter)
/BoC/	(beginning of chapter)
/EoB/	(end of book)
/BoB/	(beginning of book)

EACH QUOTED CLAUSE HAS AN ANCHOR. UNQUOTED CLAUSES ARE NEITHER ANCHORED NOR ORDERED.

For each anchor-candidate, forward-scan to completion of a match within the span, and
marking bits only upon success. As soon as a failure is noticed, call-out, but do not
advance the cursor until all clauses are forward scanned and conditionally marked
(matching words that do not consitute a matching clause won't be highlighted).

This should be no slower than earlier implementation and way more straightforward.

NEGATIVE POLARITY APPLIES TO VERSES.
THIS MIGHT BE A LITTLE TRICKY OR COUNTER INTUITIVE

Parse()
Validate()
Execute()
```

(Notes above exemplify how punctuation is used to represent Boolean expressions)  
