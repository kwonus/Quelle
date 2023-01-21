# Vanilla Quelle HMI Specification

##### version 1.0.3.121

### I. Background

Most modern search engines, provide a mechanism for searching via a text input box where the user is expected to type search terms. While primitive, this interface was pioneered by major web-search providers and represented an evolution from the far more complex interfaces that came earlier. When you search for multiple terms, however, there seems to be only one basic paradigm: “find every term”. The vast world of search is rife for a search-syntax that moves us past the ability to only search using basic search expressions. To this end, the Quelle specification is put forward an open Human-Machine-Interface (HMI) that can be invoked within a command shell or even within a simple text input box on a web page. The syntax fully supports basic Boolean operations such as AND, OR, and NOT. While great care has been taken to support the construction of complex queries, greater care has been taken to maintain a clear and concise syntax.

Quelle, IPA: [kɛl], in French means "What? or Which?". As Quelle HMI is designed to obtain search-results from search-engines, this interrogative nature befits its name. An earlier interpreter, Clarity, served as inspiration for defining Quelle.  You could think of the Quelle HMI as version 2 of the Clarity HMI specification.  However, in order to create linguistic consistency in Quelle's Human-to-Machine command language, the resulting syntax varies so significantly from the baseline specification that a new name was the best way forward.  Truly, Quelle HMI incorporates lessons learned after creating, implementing, and revising Clarity HMI for over a decade.

Every attempt has been made to make Quelle consistent with itself. Some constructs are in place to make parsing unambiguous, other constructs are biased toward ease of typing (such as limiting keystrokes that require the shift key). In all, Quelle represents an easy to type and easy to learn HMI.  Moreover, simple search statements look no different than they might appear today in a Google or Bing search box. Still, let's not get ahead of ourselves or even hint about where our simple specification might take us ;-)

Now that we understand what Quelle is, what is Vanilla Quelle?  Quelle was originally designed by AV Text Ministries to search the sacred text of the King James Bible using an inutive command language. Vanilla Quelle removes all domain bias and presents itself as a general purpose query language that can locate sections of <u>any</u> text-based documents, containing terms and/or linguistic features that are in close proximity to one another.  In this specification, any reference to Quelle implies Vanilla Quelle.
Now that we understand what Quelle is, what is Vanilla Quelle?  Quelle was originally designed by AV Text Ministries to search the sacred text of the King James Bible using an inutive command language. Vanilla Quelle removes all domain bias and presents itself as a general purpose query language that can locate sections of <u>any</u> text-based documents, containing terms and/or linguistic features that are in close proximity to one another.  In this specification, any reference to Quelle implies Vanilla Quelle.

### II. Overview

Quelle HMI maintains the assumption that proximity of terms to one another is an important aspect of searching unstructured data. Ascribing importance to the proximity between search terms is sometimes referred to as a *proximal* *search* technique. Proximal searches intentionally constrain the span of words between search terms, yet still constitute a match.

Beyond search, the specification provides a means to persist user settings and system-wide configurations. The design of Quelle is privacy-first, and is therefore not cloud-first. Consequently, settings in Quelle can easily be stored on your local system. Likewise, all processing can occur without cloud/internet computing resources.

Any application can implement the Quelle specification without royalty. We provide a [PEG]([Parsing Expression Grammar](https://en.wikipedia.org/wiki/Parsing_expression_grammar)) grammar that showcases how to harvest the meaning behind the parse. Vanilla Quelle makes no claim that it represents a ready-made interpreter.  You'll have to look elsewhere for working interpreter examples implemented atop the Quelle grammar.  The reason is, the search domain needs to be chosen in order to successfully implement an interpreter optimized for whatever domain. Consequently, Vanilla Quelle instead exposes the details of the parse in lieu of a working [DSL](https://en.wikipedia.org/wiki/Domain-specific_language) for search.

Vanilla Quelle is implemented in [Rust](https://www.rust-lang.org/) using the [Pest](https://pest.rs/) crate.  Rust source code for Vanilla Quelle and the companion PEG grammar are being shared to the community with a liberal MIT open source license.

### III. Quelle Syntax

The Quelle specification defines a declarative syntax for specifying search criteria using the *find* verb. Quelle also defines additional verbs to round out its syntax as a simple straightforward means to interact with custom applications where searching text is the fundamental problem at hand.

Quelle Syntax comprises sixteen(16) verbs. Each verb corresponds to a basic action:

- find
- filter
- set
- get
- clear
- output
- show
- save
- delete
- execute
- expand
- help
- review
- invoke
- version
- exit

Quelle is an open and extensible standard, additional verbs, and verbs for other languages can be defined without altering the overall syntax structure of the Quelle HMI. The remainder of this document describes Version 1.0 of the Quelle-HMI specification.  

In Quelle terminology, a statement is made up of one or more clauses. Each clause represents an action. While there are sixteen action-verbs, there are only six syntax categories:

1. SEARCH
   - *find*
   - *filter*
2. CONTROL
   - *set*
   - *clear*
   - @get
4. DISPLAY
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

Each syntax category has either explicit and/or implicit actions.  Explicit actions begin with the @ symbol, immediately followed by the explicit verb.  Implicit actions are inferred by the syntax of the command.

### IV. Fundamental Quelle Commands

Learning just six verbs is all that is necessary to effectively use Quelle. In the table below, each verb is identified with required and optional parameters/operators.

| Verb      | Action Type | Syntax Category | Required Parameters     | Required Operators | Optional Operators | > 1 permitted |
| --------- | :---------: | :-------------- | ----------------------- | :----------------: | :----------------: | :-----------: |
| *find*    |  implicit   | SEARCH          | **1**: *search spec*    |                    |   **" "  \|  &**   |      yes      |
| *filter*  |  implicit   | SEARCH          | **1**: *filter spec*    |  **\\**spec**\\**  |                    |      yes      |
| *set*     |  implicit   | CONTROL         | **2**: *name* = *value* |       **=**        |                    |      yes      |
| *show*    |  implicit   | DISPLAY         | 0                       |     **\|\|**       |      **[ ]**       |      no       |
| **@help** |  explicit   | SYSTEM          | 0 or 1                  |                    |                    |      no       |
| **@exit** |  explicit   | SYSTEM          | 0                       |                    |                    |      no       |

**TABLE 4-1** -- **The six fundamental Quelle commands with corresponding syntax summaries**

From a linguistic standpoint, all Quelle commands are issued in the imperative. The subject of the verb is always "you understood". As the user, you are commanding Quelle what to do. Some verbs have direct objects [aka required parameters]. These parameters instruct Quelle <u>what</u> to act upon. The syntax category of the verb dictates the required parameters.

Quelle supports two types of actions:

1. Implicit actions [implicit actions are inferred from the syntax of their parameters]
2. Explicit actions [The verb needs to be explicitly stated in the command and it begins with **@**]

Implicit actions can be combined into compound statements.  However, compound statements are limited to contain ONLY implicit actions. This means that explicit actions cannot be used to construct a compound statement.

Constructing a compound statement with multiple implicit actions, involves delimiting each action with semi-colons. As search is a fundamental concern of Quelle, it is optimized to make compound implicit actions easy to construct with a concise and and intuitive syntax.

Even before we describe Quelle syntax generally, let's examine these concepts using examples:

| Description                             | Example                                  |
| --------------------------------------- | :--------------------------------------- |
| SYSTEM command                          | @help                                    |
| SEARCH filter                           | \\wall street journal : 2022-07-04\\     |
| SEARCH specification                    | this is some text expected to be found   |
| Compound statement: two SEARCH actions  | "this quoted text" ; other unquoted text |
| Compound statement: two CONTROL actions | span=7 ; exact = true                    |
| Compound statement: CONTROL & SEARCH    | span=7; "Moses said"                     |

**TABLE 4-2** -- **Examples of Quelle statement types**

Consider these two examples of Quelle statements (first CONTROL; then SEARCH):

search.domain = wall street journal

"Kamala Harris"

Notice that both statements above are single actions.  We should have a way to express both of these in a single command. And this is the rationale behind a compound statement. To combine the previous two actions into one compound statement, issue this command:

"Kamala Harris" ; search.domain=wall street journal

### V. Deep Dive into Quelle SEARCH actions

Consider this proximity search where the search using Quelle syntax:

*domain=wall street journal ; Harris Biden*

Quelle syntax can alter the span by also supplying an additional CONTROL action:

*domain=wall street journal ; span=8 ; Harris Biden*

The statement above has two CONTROL actions and one SEARCH action

Next, consider a search to find Biden and (Kamala or Harris):

*Biden Kamala|Harris*

The order in which the search terms are provided is insignificant. Additionally, the type-case is insignificant. 

Of course, there are times when word order is significant. Accordingly, searching for explicit strings can be accomplished using double-quotes as follows:

*"Biden said ... Harris"*

These constructs can even be combined. For example:

*"Biden said ... Kamala|Harris"*

The search criteria above is equivalent to this search:

*"Biden said ... Kamala" + "Biden said ... Harris"*

In all cases, “...” means “followed by”, but the ellipsis allows other words to appear between "said" and "Kamala". Likewise, it allows words to appear between "said" and "Harris". 

Of course, translating the commands into actual search results might not be trivial for the application developer. Still, the Vanilla Quelle parser and the PEG grammar are freely available, allowing the developer to just leverage the parse and focus on delivering search results.

Quelle is designed to be intuitive. It provides the ability to invoke Boolean logic on how term matching should be performed. As we saw above, the pipe symbol ( | ) can be used to invoke an OR condition [Boolean multiplication upon the terms that compose a search expression].

The ampersand symbol can similarly be used to represent AND conditions upon terms. If we were searching for how a baseball archive, we might issue this command:

"Babe Ruth ... home run&/noun/"

If the corpus is marked for part-of-speech, this search would return only matching phrases where the word run was labelled as a noun.

Of course, part-of-speech expressions can also be used independent of the an AND condition, as follows:

span = 6 ; "/noun/ ... home run"

This would find phrases where a noun appeared within a span of six words and preceded "home run"

**Another SEARCH Example:**

Consider a query for all passages that contain a word beginning with pres, followed by Bush, but filter out phrases containing H W Bush.

*span = 15 ; "Pres*\* ... Bush" -- "H W Bush"*

*(this could be read as: find all references to a wildcard Pres [e.g. Pres. or President] Bush, but filter out [i.e. subtract] phrases that also contain "HW Bush"*

### VI. Displaying Results

Consider that there are two fundamental types of searches:

- Searches that return a limited set of results
- Searches that either return loads of results; or searches where the result count is unknown (and potentially very large)

Due to the latter condition above, SEARCH summarizes results (it does NOT display every result found). However, if more than a summary is desired, the user can control how many results to display.

"Clinton answered"			*summarize documents that contain this phrase, with paragraph references*

"Clinton answered" ||        *display every matching phrase* // equivalent to Clinton answered" ||[\*]

"Clinton answered" || [1]  *this would would display only the first matching phrase*

"Clinton answered" || [1 2 3]  *this would would display only the first three matching phrases*

"Clinton answered" || [4 5 6]  *this would would display the next first three matching phrases*

### VII. Exporting Results

Export using a display-coordinate:

*\\* wall street journal : 2022-07-04 *\\*

To revisit the example in the previous sample, we can export records to a file with these commands:

"Clinton answered" || > my-file.output  // *this would export every matching phrase*

"Clinton answered" || [1]  > my-file.output  // *this would would export only the first matching phrase*

"Clinton answered" || [1 2 3]  >> my-file.output  //  *this would would export the first three matching phrases* // >> indicates that the results should be appended

format=html ; "Clinton answered" || [1 2 3]  > my-file.html ! // *export the first three matching phrases as html*

The trailing exclamation point allows existing file to be overwritten

### VIII. Program Help

*@help*

This will provide a help message in a Quelle interpreter.

Or for specific topics:

*@help* find

*@help* set

@help output

etc ...

### IX. Exiting Quelle

Type this to terminate the Quelle interpreter:

*@exit*

### X. Additional actions

| Verb         | Action Type | Syntax Category | Required Parameters     |    Required Operators     | Optional Operators | > 1 permitted |
| ------------ | :---------: | --------------- | ----------------------- | :-----------------------: | :----------------: | :-----------: |
| *clear*      |  implicit   | CONTROL         | **2**: *name*           |          **=@**           |                    |      yes      |
| *output*     |  implicit   | DISPLAY         | **1**: *filename*       | **>** or **>>** or **>!** |                    |      no       |
| **@get**     |  explicit   | CONTROL         | **0+**: *control_names* |                           |                    |      no       |
| **@version** |  explicit   | SYSTEM          | **0**                   |                           |                    |      no       |

**TABLE 10-1** -- **Listing of additional CONTROL, DISPLAY & SYSTEM actions**

**CONTROL::SETTING directives:**

| **Markdown**          | **HTML**                | **Text**                |
| --------------------- | ----------------------- | ----------------------- |
| *display.format = md* | *display.format = html* | *display.format = text* |

**TABLE 10-2** -- **set** format command can be used to set the default content-formatting for for use with the export verb



| **example**            | **explanation**                                              |
| ---------------------- | ------------------------------------------------------------ |
| *search*.span = 8      | Assign a control setting                                     |
| **@get** *search*.span | get a control setting                                        |
| *search*.span=@        | Clear the control setting; restoring the Quelle driver default setting |

**TABLE 10-3** -- **set/clear/get** action operate on configuration settings



**CONTROL::REMOVAL directives:**

When *clear* verbs are used alongside *set* verbs, *clear* verbs are always executed after *set* verbs. 

span=@ ; span = 7  `>> implies >>` span=@

Otherwise, when multiple clauses contain the same setting, the last setting in the list is preserved.  Example:

format = md ;  format = text `>> implies >>` format = text

The control names are applicable to ***set***, ***clear***, and ***@get*** verbs. The control name has a fully specified name and also a short name. Either form of the control name is permitted in all Quelle statements.

| Fully Specified Name | Short Name | Meaning                      | Values         | Visibility |
| -------------------- | ---------- | ---------------------------- | -------------- | :--------: |
| search.span          | span       | proximity                    | 0 to 1000      |   normal   |
| search.domain        | domain     | the domain of the search     | string         |   normal   |
| search.exact         | exact      | exact match vs liberal/fuzzy | true/false     |   normal   |
| display.heading      | heading    | heading of results           | string         |   normal   |
| display.record       | record     | annotation of results        | string         |   normal   |
| display.format       | format     | format of results            | see Table 10-2 |   normal   |

**TABLE 10-4** -- **Summary of standard Quelle Control Names**

Table 10-4 lists Control-Names for SEARCH and DISPLAY actions. The *@get* command will list the values associated with these. The *@get* command takes zero or more arguments. Zero arguments lists all control settings.  With one or more arguments, get only lists the values of the controls that are specified.  Examples of the command are below (both the long form and the short form of control names are accepted):

*@get*

*@get* search

*@get* search.domain

Control settings can be cleared using implicit wildcards, by using the shared control-prefix:

search=@

display=@

For example, this control statement with an implied wildcard:

search=@

It is exactly equivalent to this compound statement:

search.span=@ ; search.domain=@ ; search.exact=@

Likewise, it is exactly equivalent to this similar compound statement:

span=@ ; domain=@ ; exact=@

**QUERYING DRIVER FOR VERSION INFORMATION**

@version will query the Quelle Search Provider for version information:

### XI. Reviewing Statement History and re-invoking statements

| Verb        | Action Type | Syntax Category | Required Arguments | Required Operators | > 1 permitted |
| ----------- | ----------- | --------------- | ------------------ | :----------------: | :-----------: |
| **@review** | explicit    | HISTORY         | 0 or 1             |                    |      no       |
| *invoke*    | implicit    | HISTORY         | 1: historic-id     |      **{ }**       |      no       |

##### **TABLE 11-1 -- Reviewing history and re-invoking previous commands**

**REVIEW SEARCH HISTORY**

*@review* gets the user's search activity for the current session.  To show the last ten searches, type:

*@review*

To show the last three searches, type:

*@review* 3

To show the last twenty searches, type:

*@review* 20 

**INVOKE**

The *invoke* command works a little bit like a macro, albeit with different syntax.  After executing a *@review* command, the user might receive a response as follows.

*@review*

1>  span = 7

2>  exact = true

3> eternal power

And the invoke command can re-invoke any command listed.

{3}

would be shorthand to re-invoke the search specified as:

eternal power

or we could re-invoke all three commands in a single statement as:

{1} ; {2} ; {3}

which would be interpreted as:

span = 7 ; exact = true ; eternal power

### XII. Labelling Statements for subsequent execution

| Verb        | Action Type | Syntax Category | Required Arguments     | Required Operators | Optional Operators | > 1 permitted |
| ----------- | ----------- | --------------- | ---------------------- | :----------------: | :----------------: | :-----------: |
| *save*      | implicit    | LABEL           | **1**: *macro_label*   |       **<<**       |                    |      no       |
| **@delete** | independent | LABEL           | **1+**: *macro_label*s |      **{ }**       |                    |      no       |
| **@expand** | independent | LABEL           | **0+**: *macro_labels* |                    |      **{ }**       |      no       |
| *execute*   | implicit    | LABEL           | 1: *macro_label*       |      **{ }**       |                    |      yes      |

**TABLE 12-1** -- **Executing labelled statements and related commands**

In this section, we will examine how user-defined macros are used in Quelle.  A macro in Quelle is a way for the user to label a statement for subsequent use.  By applying a label to a statement, a shorthand mechanism is created for subsequent execution. This gives rise to two new definitions:

1. Labelling a statement (or defining a macro)

2. Utilization of a labelled statement (executing a macro)


Let’s say we want to name our previously identified SEARCH directive with a label; We’ll call it “kh”. To accomplish this, we would issue this command:

"Kamala Harris" ; search.domain=wall street journal << kh

It’s that simple, now instead of typing the entire statement, we can use the label to execute our newly saved statement. Here is how we would execute the macro:

{kh}

Labelled statements also support compounding using the semi-colon ( ; ), as follows; we will label it also:

{kh} ; "former President Bush" << my label can contain spaces

Later I can issue this command:

{my label can contain spaces}

Which is obviously equivalent to executing these labeled statements:

"Kamala Harris" ; search.domain=wall street journal ; "former President Bush"

To illustrate this further, here are four more examples of labeled statement definitions:

search.exact=1 << C1

search.span=8  << C2

nationality << F1

eternal  << F2

We can execute these as a compound statement by issuing this command:

{C1} ; {C2} ; {F1} ; {F2}

Similarly, we could define another label from these, by issuing this command:

{C1} ; {C2} ; {F1} ; {F2} << sample-compound-macro

This expands to:

search.exact = 1  search.span = 8 ; nationality ; eternal

There are two restrictions on macro definitions:

1. Macro definition must represent a valid Quelle statements:
   - The syntax is verified prior to saving the statement label.
2. The statement cannot contain explicit actions:
   - Only implicit actions are permitted in a labelled statement.

Finally, there are two additional ways that a labelled statement can be referenced. In the last macro definition above where we created {sample2}, the user could see the expansion in Quelle by issuing this command:

@list {sample-compound-macro}

If the user wanted to remove this definition, the @delete action is used.  Here is an example:

@delete {sample-compound-macro}

NOTE: Labels must begin with a letter [A-Z] or [a-z], but they may contain numbers, spaces, hyphens, periods, commas, underscores, and single-quotes (no other punctuation or special characters are supported).

### Appendix A. Glossary of Quelle Terminology

**Syntax Categories:** Each syntax category defines rules by which verbs can be expressed in the statement. 

**Actions:** Actions are complete verb-clauses issued in the imperative [you-understood].  Many actions have one or more parameters.  But just like English, a verb phrase can be a single word with no explicit subject and no explicit object.  Consider this English sentence:

Go!

The subject of this sentence is "you understood".  Similarly, all Quelle verbs are issued without an explicit subject. The object of the verb in the one word sentence above is also unstated.  Quelle operates in an analogous manner.  Consider this English sentence:

Go Home!

Like the earlier example, the subject is "you understood".  The object this time is defined, and insists that "you" should go home.  Some verbs always have objects, others sometimes do, and still others never do. Quelle follows this same pattern and some Quelle verbs require direct-objects; and some do not.  See Table 4-1 where the column identified as "Parameter Count" identifies objects of the verb. 

**Statement**: A statement is composed of one or more *actions*. If there is more than one SEARCH actions issued by the statement, then search action is logically OR’ed together.

**Unquoted SEARCH clauses:** an unquoted search clause contains one or more search words. If there is more than one word in the clause, then each word is logically AND’ed together. Like all other types of clauses, the end of the clause terminates with a semicolon, a plus-sign, or a linefeed.

- ; [semi-colon]
- \+ [plus-sign]
- the end-of-the-line [newline]

**Quoted SEARCH clauses:** a quoted clause contains a single string of terms to search. An explicit match on the string is required. However, an ellipsis ( … ) can be used to indicate that wildcards may appear within the quoted string.

**NOTES:**

- It is called *quoted,* as the entire clause is sandwiched on both sides by double-quotes ( " )
- The absence of double-quotes means that the statement is unquoted

**Bracketed Terms:** When searching, there are part the order of some terms within a quoted are unknown. Square brackets can be used to identify such terms. For example, consider this SEARCH statement:

*“noun ... said|says|stated|replied ... [Trump president|predidency]”*

Bracketed terms differ from the pipe symbol in that all terms within brackets are required, but the order is relaxed within the quoted phrase. As non-quoted clauses are always unordered, bracketed terms can only be invoked within a quoted search clause. 

The serch clause above would match any of these phrases:

- Biden stated that President Trump was ...
- Kamala Harris replied, indicating the Trump presidency was ...

These phrase would NOT match:

- Bush said, Trump is not his favorite president // Bracketed terms need be adjacent to one another

**and:** In Boolean logic, **and** means that all terms must be found. With Quelle, *and* is represented by terms that appear within an unquoted clause. 

**or:** In Boolean logic, **or** means that any term constitutes a match. With Quelle, *or* is represented by the semi-colon ( **;** ) or plus (+) between SEARCH clauses. 

**not:** In Boolean logic, **not** means that the term must not be found. With Quelle, *not* is represented by a minus, minus ( **--** ) and applies to an entire clause (it cannot be applied to individual segments (e.g. discrete words) within the search clause. However, a search clause is permitted to contain a single segment, which easily circumvents that limitation. In short, -- means subtract results; it cancels-out matches against all matches of other clauses. Most clauses are additive as each additional clause increases search results. Contrariwise, a **not** clause is subtractive as it decreases search results.

Again, -- means that the clause will be subtracted from the search results.. When commands only contain a single search clause, it is always positive. A single negative clause following the find imperative, while it might be grammatically valid syntax, will never match anything. Therefore, while permitted in theory, it would have no real-world meaning. Consequently, most implementations of Quelle-HMI disallow that construct.
