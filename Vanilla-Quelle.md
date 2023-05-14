# Quelle Specification

##### version 2.0.3.512

### I. Background

Most modern search engines, provide a mechanism for searching via a text input box where the user is expected to type search terms. While primitive, this interface was pioneered by major web-search providers and represented an evolution from the far more complex interfaces that came earlier. When you search for multiple terms, however, there seems to be only one basic paradigm: “find every term”. The vast world of search is rife for a search-syntax that moves us past the ability to only search using basic search expressions. To this end, the Quelle specification is put forward an open Human-Machine-Interface (HMI) that can be invoked within a command shell or even within a simple text input box on a web page. The syntax fully supports basic Boolean operations such as AND, OR, and NOT. While great care has been taken to support the construction of complex queries, greater care has been taken to maintain a clear and concise syntax.

Quelle, IPA: [kɛl], in French means "What? or Which?". As Quelle HMI is designed to obtain search-results from search-engines, this interrogative nature befits its name. An earlier interpreter, Clarity, served as inspiration for defining Quelle.  You could think of the Quelle HMI as an evolution of Clarity HMI.  However, in order to create linguistic consistency in Quelle's Human-to-Machine command language, the resulting syntax varied so dramatically from the Clarity spec, that a new name was the best way forward.  Truly, Quelle HMI incorporates lessons learned after creating, implementing, and revising Clarity HMI for over a decade.

In 2023, Quelle-AVX 2.0 was defined. This new specification is not a radical departure from version 1. Most of the changes are center around macros, control variables, filters, and export directives. Search syntax has remained almost entirely unchanged. It turned out that my reference PEG grammar had some  ambiguity differentiating between the various implicit actions. To eliminate that ambiguity, new operators were introduced  [We added $ % and || to name a few] . These additions also reduced the need for clause delimiters. The version 2 spec feels more streamlined, more intuitive, and comes with a working revision of the PEG grammar. Implicit actions for Macros have been reclassified as *apply* and *utilize* [those verbs replace *save* and *execute* respectively] .  These new verbs align with the metaphor of labelling.  As Quelle-AVX is the only current known reference implementation of Quelle, it is likely that Vanilla-Quelle will eventually undergo this same evolution.

The one change to the search specification in Quelle 2.0 is the dropping of support for bracketed search terms. While parsing these artifacts was easy with the PEG grammar, implementing the search from the parse was quite complex.

An upcoming feature, that provides some overlapping functionality with that deprecated spec is discussed in more detail in the ***Filtering Results*** section of this document.

Every attempt has been made to make Quelle consistent with itself. Some constructs are in place to make parsing unambiguous, other constructs are biased toward ease of typing (such as limiting keystrokes that require the shift key). Of course, other command languages also influence our syntax, to make things more intuitive for a polyglot. In all, Quelle represents an easy to type and easy to learn HMI.  Moreover, simple search statements look no different than they might appear today in a Google or Bing search box. Still, let's not get ahead of ourselves or even hint about where our simple specification might take us ;-)

### II. Addendum

A reference implementation of Quelle can be found in Quelle-AVX and related open source projects. There are two possible implementation levels:

- Level 1 [basic search support]
- Level 2 [search support includes also searching on part-of-speech tags]

### III. Quelle Syntax

Quelle defines a declarative syntax for specifying search criteria using the *find* verb. Quelle also defines additional verbs to round out its syntax as a simple straightforward means to interact with custom applications where searching text is the fundamental problem at hand.

Quelle Syntax comprises seventeen(17) verbs. Each verb corresponds to a basic action:

- find
- filter
- set
- get
- clear
- initialize
- export
- show
- apply
- delete
- utilize
- expand
- help
- review
- invoke
- version
- exit

Quelle is an open and extensible standard, additional verbs, and verbs for other languages can be defined without altering the overall syntax structure of the Quelle HMI. The remainder of this document describes Version 1.0 of the Quelle-HMI specification.  

In Quelle terminology, a statement is made up of one or more clauses. Each clause represents an action. While there are seventeen action-verbs, there are only six syntax categories:

1. SEARCH
   - *find*
   - *filter*
2. CONTROL
   - *set*
   - *clear*
   - @get
   - @reset *(explicit alias for "**clear all control settings**")*
3. OUTPUT
   - *show*
   - *export*
4. SYSTEM
   - @help
   - @version
   - @exit
5. HISTORY
   - *invoke*
   - @review
   - @initialize
6. LABEL
   - *apply*
   - *utilize*
   - @delete
   - @expand

Each syntax category has either explicit and/or implicit actions.  Explicit actions begin with the @ symbol, immediately followed by the explicit verb.  Implicit actions are inferred by the syntax of the command.

### IV. Fundamental Quelle Commands

Learning just six verbs is all that is necessary to effectively use Quelle. In the table below, each verb is identified with required and optional parameters/operators.

| Verb      | Action Type | Syntax Category | Required Parameters       |  Alternate Parameters   |
| --------- | :---------: | :-------------- | ------------------------- | :---------------------: |
| *find*    |  implicit   | SEARCH          | *search spec*             |                         |
| *filter*  |  implicit   | SEARCH          | **<** *domain*            |                         |
| *set*     |  implicit   | CONTROL         | **%name** **::** *value*  | **%name** **=** *value* |
| *show*    |  implicit   | OUTPUT          | **[** *row_indices* **]** |                         |
| **@help** |  explicit   | SYSTEM          |                           |         *topic*         |
| **@exit** |  explicit   | SYSTEM          |                           |                         |

**TABLE 4-1** -- **The six fundamental Quelle commands with corresponding syntax summaries**

From a linguistic standpoint, all Quelle commands are issued in the imperative. The subject of the verb is always "you understood". As the user, you are commanding Quelle what to do. Some verbs have direct objects [aka required parameters]. These parameters instruct Quelle <u>what</u> to act upon. The syntax category of the verb dictates the required parameters.

Quelle supports two types of actions:

1. Implicit actions [implicit actions are inferred from the syntax of their parameters]
2. Explicit actions [The verb needs to be explicitly stated in the command and it begins with **@**]

Implicit actions can be combined into compound statements.  However, compound statements are limited to contain ONLY implicit actions. This means that explicit actions cannot be used to construct a compound statement.

As search is a fundamental concern of Quelle, it is optimized to make compound implicit actions easy to construct with a concise and intuitive syntax. Even before we describe Quelle syntax generally, let's examine a few concepts using examples:

| Description                             | Example                                  |
| --------------------------------------- | :--------------------------------------- |
| SYSTEM command                          | @help                                    |
| SEARCH filters                          | < Genesis < Exodus < Revelation          |
| SEARCH specification                    | this is some text expected to be found   |
| Compound statement: two SEARCH actions  | "this quoted text" + other unquoted text |
| Compound statement: two CONTROL actions | %span = 7 %exact = true                  |
| Compound statement: CONTROL & SEARCH    | %span=7 "Moses said"                     |

**TABLE 4-2** -- **Examples of Quelle statement types**

Consider these two examples of Quelle statements (first CONTROL; then SEARCH):

%domain = wall street journal

"Kamala Harris"

Notice that both statements above are single actions.  We should have a way to express both of these in a single command. And this is the rationale behind a compound statement. To combine the previous two actions into one compound statement, issue this command:

"Kamala Harris" %domain=wall street journal

### V. Deep Dive into Quelle SEARCH actions

Consider this proximity search where the search using Quelle syntax:

*domain=wall street journal + Harris Biden*

Quelle syntax can alter the span by also supplying an additional CONTROL action:

*domain=wall -street-journal span=8 Harris Biden*

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

The ampersand symbol can similarly be used to represent AND conditions upon terms. If we were searching a dataset in a baseball domain, we might issue this command:

"Babe Ruth ... home run&/noun/"

If the corpus is marked for part-of-speech, this search would return only matching phrases where the word run was labelled as a noun.

Of course, part-of-speech expressions can also be used independent of the an AND condition, as follows:

span = 6  "/noun/ ... home run"

This would find phrases where a noun appeared within a span of six words and preceded "home run"

**Another SEARCH Example:**

Consider a query for all passages that contain a word beginning with pres, followed by Bush, but subtract phrases containing H W Bush.

*span = 15 ; "Pres*\* ... Bush" -- "H W Bush"*

*(this could be read as: find all references to a wildcard Pres [e.g. Pres. or President] Bush, but subtract phrases that also contain "HW Bush"*

**Valid statement syntax, but no results:**

this&that

/noun/ & /verb/

Both of the statements above are valid, but will not match any results. Search statements attempt to match actual words in  the actual bible text. A bord cannot be "this" **and** "that". Likewise, an individual word in a sentence does not operate as a /noun/ **and** a /verb/. Contrariwise, these searches are valid, but would also return numerous matches:

this|that

/noun/ | /verb/

### VI. Displaying Results

Consider that there are two fundamental types of searches:

- Searches that return a limited set of results
- Searches that either return loads of results; or searches where the result count is unknown (and potentially very large)

Due to the latter condition above, SEARCH summarizes results (it does NOT display every result found). However, if more than a summary is desired, the user can control how many results to display.

"Clinton answered"			*summarize documents that contain this phrase, with paragraph references*

"Clinton answered" [  ]        *display every matching phrase* // equivalent to Clinton answered" ||[\*]

"Clinton answered" [1]  *this would would display only the first matching phrase*

"Clinton answered" [1 2 3]  *this would would display only the first three matching phrases*

"Clinton answered" [4 5 6]  *this would would display the next first three matching phrases*

### VII. Exporting Results

Export using a display-coordinate:

To revisit the example in the previous sample, we can export records to a file with these commands:

"Clinton answered" [ ]  > my-file.output  // *this would export every matching phrase*

"Clinton answered"  [1]  > my-file.output  // *this would would export only the first matching phrase*

"Clinton answered"  [1 2 3]  >> my-file.output  //  *this would would export the first three matching phrases* // >> indicates that the results should be appended

format=html ; "Clinton answered"  [1 2 3]  => my-file.html // *export the first three matching phrases as html*

The => allows existing file to be overwritten. Quelle will not overwrite an existing file with > syntax. The => is required to force an overwrite.

### VIII.  Labelling Statements for subsequent utilization

| Verb        | Action Type | Syntax Category | Required Arguments | Required Operators |
| ----------- | ----------- | --------------- | ------------------ | :----------------: |
| *apply*     | implicit    | LABEL           | **1**: *label*     |  **\|\|** *label*  |
| **@delete** | independent | LABEL           | **1+**: *label*s   |      *label*       |
| **@expand** | independent | LABEL           | **1**: *label      |      *label*       |
| *utilize*   | implicit    | LABEL           | **1+**: *labels*   |   **$** *label*    |

**TABLE 8-1** -- **Utilizing labelled statements and related commands**

In this section, we will examine how user-defined macros are used in Quelle.  A macro in Quelle is a way for the user to label a statement for subsequent use.  By applying a label to a statement, a shorthand mechanism is created for subsequent utilization. This gives rise to two new definitions:

1. Labelling a statement (or defining a macro)

2. Utilization of a labelled statement (running a macro)


Let’s say we want to name the search example from the previous section; We’ll call it *eternal-power*. To accomplish this, we would issue this command:

%span::7 %exact::true + eternal power || eternal-power

It’s that simple, now instead of typing the entire statement, we can utilize the macro by referencing our previously applied label. Here is how the macro can be utilized. We might call this running the macro:

$eternal-power

Labelled statements also support compounding using the semi-colon ( ; ), as follows; we will label it also:

$eternal-power + diety|| my-label-cannot-contain-spaces

Later I can issue this command:

$my-label-cannot-contain-spaces

Which is equivalent to these statements:

%span::7  %exact::true + eternal power + diety

To illustrate this further, here are four more examples of labeled statement definitions:

%exact::1 || C1

%span::8  || C2

+nation || F1

+eternal  || F2

We can utilize these as a compound statement by issuing this command:

$C1  $C2  $F1  $F2

Similarly, we could define another label from these, by issuing this command:

$C1 $C2  $F1  $F2 || another-macro

This expands to:

%exact::1  %span::8   nation + eternal

There are several restrictions on macro definitions:

1. Macro definition must represent a valid Quelle statement:
   - The syntax is verified prior to applying the statement label.
2. Macro definitions exclude export directives
   - Any portion of the statement that contains > is incompatible with a macro definition
3. Macro definitions exclude output directives
   - Any portion of the statement that contains [ ] is incompatible with a macro definition
4. The statement cannot represent an explicit action:
   - Only implicit actions are permitted in a labelled statement.

Finally, any macros referenced within a macro definition are expanded prior to applying the new label. Therefore redefining a macro after it has been referenced in a subsequent macro definition has no effect of the initial macro reference. We call this macro-determinism.  A component of determinism for macros is that the macro definition saves all control settings at the time that the label was applied. This assure that the same search results are returned each time the macro is referenced. However, if the user desires the current settings to be used instead, just ***::current*** suffix after the macro. 

##### Additional explicit macro commands:

Two additional explicit commands exist whereby a macro can be manipulated. We saw above how they can be defined and referenced. There are two additional ways commands that operate on macros: expansion and deletion.  In the last macro definition above where we created  $another-macro, the user could preview an expansion by issuing this command:

@expand another-macro

If the user wanted to remove this definition, the @delete action is used.  Here is an example:

@delete another-macro

NOTE: Labels must begin with a letter [A-Z] or [a-z], but they may contain numbers, spaces, hyphens, periods, commas, underscores, and single-quotes (no other punctuation or special characters are supported).

While macro definitions are deterministic, they can be overwritten/redefined.

### IX. Reviewing Statement History and re-invoking statements

| Verb            | Action Type | Required Parameter | Optional Parameters |
| --------------- | ----------- | :----------------: | :-----------------: |
| *invoke*        | implicit    |     **$** *id*     |                     |
| **@review**     | explicit    |                    |     *max_coun*t     |
| **@initialize** | explicit    |      history       |                     |

##### **TABLE 9-1 -- Reviewing history and re-invoking previous commands**

**REVIEW SEARCH HISTORY**

*@review* gets the user's search activity for the current session.  To show the last ten searches, type:

*@review*

To show the last three searches, type:

*@review* 3

To show the last twenty searches, type:

*@review* 20 

**INVOKE**

The *invoke* command works a little bit like a macro, albeit with different syntax.  After invoking a *@review* command, the user might receive a response as follows.

*@review*

1>  %span = 7

2>  %exact = true

3> eternal power

And the invoke command can re-invoke any command listed.

$3

would be shorthand to re-invoke the search specified as:

eternal power

*Invoking* command history is very much analogous with *utilizing* a macro. Just like a macro, the control settings are saved to provide determinism. That means that the current control settings are ignored when invoking command history. Just like with macros, the current control settings can be utilized by adding the ***::current*** suffix to the invocation. See **Table 12-5**. Example usage:

$3::current

or we could re-invoke all three commands in a single statement as:

$1  $2  $3

which would be interpreted as:

%span = 7  %exact = true   eternal power

**RESETTING COMMAND HISTORY**

The @reset command can be used to clear command history and/or reset all control variables to defaults.

To clear all command history:

@initialize history

### X. Program Help

*@help*

This will provide a help message in a Quelle interpreter.

Or for specific topics:

*@help* find

*@help* set

@help export

etc ...

### XI. Exiting Quelle

Type this to terminate the Quelle interpreter:

*@exit*

### XII. Control Settings

| Verb     | Action Type | Syntax Category | Parameters             | Alternate #1          | Alternate #2      |
| -------- | :---------: | --------------- | ---------------------- | :-------------------- | :---------------- |
| *clear*  |  implicit   | CONTROL         | *%name* **:: default** | *%name* **= default** |                   |
| *export* |  implicit   | OUTPUT          | **>** *filename*       | **=>** *filename*     | **>>** *filename* |
| **@get** |  explicit   | CONTROL         | *name*                 |                       |                   |

**TABLE 12-1** -- **Listing of additional CONTROL, OUTPUT & SYSTEM actions**

**Export Format Options:**

| **Markdown**   | **HTML**         | **Text**         | Json             |
| -------------- | ---------------- | ---------------- | ---------------- |
| *%format = md* | *%format = html* | *%format = text* | *%format = json* |

**TABLE 12-2** -- **set** format command can be used to set the default content-formatting for for use with the export verb



| **example**    | **explanation**                                              |
| -------------- | ------------------------------------------------------------ |
| span = 8       | Assign a control setting                                     |
| **@get** span  | get a control setting                                        |
| span = default | Clear the control setting; restoring the Quelle driver default setting |

**TABLE 12-3** -- **set/clear/get** action operate on configuration settings



**SETTINGS:**

Otherwise, when multiple clauses contain the same setting, only the last setting in the list is preserved.  Example:

format=md   format=default  format=text

@get format

The final command would return text.  We call this: "last assignment wins". However, there is one caveat to this precedence order: regardless of where in the statement a macro or history invocation is provided within a statement, it never has precedence over a setting that is actually visible within the statement.  Consider this sequence as an example:

%exact = false || precedence_example

%exact = true  $precedence_example

@get exact

The final command would return true, because it was visible in the compound statement.

Quelle-AVX manifests four control names. Each allows all three actions: ***set***, ***clear***, and ***@get*** verbs. Table 12-4 lists all settings available in AVX-Quelle.

| Setting | Meaning                       | Values            | Default Value |
| ------- | ----------------------------- | ----------------- | ------------- |
| span    | proximity distance limit      | 0 to 999 or verse | 0 [verse]     |
| exact   | exact match vs multi-matching | true/false        | false         |
| format  | format of results             | see Table 12-2    | json          |

**TABLE 12-4** -- **Summary of Quelle-AVX Control Names**

The *@get* command fetches these values. The *@get* command requires a single argument. Examples are below:

*@get* search

@get format

There are additional actions that affect all control settings collectively

| Expressions  | Meaning / Usage                                              |
| ------------ | ------------------------------------------------------------ |
| **@reset**   | Reset is an explicit command alias to *clear* all control settings, resetting them all to default values<br />(persistent scope: equivalent to span=default lexicon=default exact=default format=default) |
| $X::defaults | Special suffix for use with History or Macro executed as a singleton statement:<br />See "Labelling Statements for subsequent utilization" section of this document.<br />Uses default settings for invocation/utilization on history/macro identified/labelled as "X". |
| $X::current  | Special suffix for use with History or Macro executed as a singleton statement:<br />See "Labelling Statements for subsequent utilization" section of this document.<br />Uses current settings for invocation/utilization on history/macro identified/labelled as "X". |
| $X::absorb   | Special suffix for use with History or Macro executed as a singleton statement.<br />See "Labelling Statements for subsequent utilization" section of this document.<br />In lieu of invocation/utilization, this command absorbs all settings for future statements<br />on history/macro identified/labelled as "X". |

**TABLE 12-5** -- **Collective CONTROL operations**

All settings can be cleared using an explicit command:

@reset controls

It is exactly equivalent to this compound statement:

%span=default  %lexicon=default  %exact=default  %format=default

**Scope of Settings [Statement Scope vs Persistent Scope]**

It should be noted that there is a distinction between name=value and name::value syntax variations. The first form is persistent with respect to subsequent statements. Contrariwise, the latter form affects only the single statement wherewith it is executed. We refer to this as variable scope, Consider these two very similar command sequences:

| Example of Statement Scope | Example of Persistent Scope |
| -------------------------- | --------------------------- |
| @reset controls            | @reset controls             |
| "Moses said" %span::7      | "Moses said" %span = 7      |
| "Aaron said"               | "Aaron said"                |

In the **statement scope** example, setting span to "7" only affects the search for "Moses said". The next search for "Aaron said" utilizes the default value for span, which is "verse".

In the **persistent scope** example, setting span to "7" affects the search for "Moses said" <u>and all subsequent searches</u>. The next search for "Aaron said" continues to use a span value of "7'".   In other words, the setting **persists** <u>beyond the scope of statement execution</u>.

### XIII. Miscellaneous Information

| Verb         | Action Type | Syntax Category |
| ------------ | :---------: | --------------- |
| **@version** |  explicit   | SYSTEM          |

**QUERYING DRIVER FOR VERSION INFORMATION**

@version will query the Quelle Search Provider for version information:

Finally delimiters ( e.g.  ; or + ), between two distinct unquoted search clauses are ***required*** to identify the boundary between the two search clauses. A single delimiters <u>before</u> any search clause is always ***optional*** (e.g., Delimiters are permitted between quoted and unquoted clauses, and any other clause that precedes any search clause). They are ***unexpected*** anywhere else in the statement. This makes a Quelle statement a bit less cluttered than the version 1.0 specification

---

An object model that manifests a representation of Quelle grammar is depicted below:

![QCommand](C:\src\Quelle\QCommand.png)

### Appendix A. Glossary of Quelle Terminology

**Syntax Categories:** Each syntax category defines rules by which verbs can be expressed in the statement. 

**Actions:** Actions are complete verb-clauses issued in the imperative [you-understood].  Many actions have one or more parameters.  But just like English, a verb phrase can be a single word with no explicit subject and no explicit object.  Consider this English sentence:

Go!

The subject of this sentence is "you understood".  Similarly, all Quelle verbs are issued without an explicit subject. The object of the verb in the one word sentence above is also unstated.  Quelle operates in an analogous manner.  Consider this English sentence:

Go Home!

Like the earlier example, the subject is "you understood".  The object this time is defined, and insists that "you" should go home.  Some verbs always have objects, others sometimes do, and still others never do. Quelle follows this same pattern and some Quelle verbs require direct-objects; and some do not.  In the various tables throughout this document, required and optional parameters are identified, These parameters represent the object of the verb within each respective table.

**Statement**: A statement is composed of one or more *actions*. If there is more than one SEARCH actions issued by the statement, then search action is logically OR’ed together.

**Unquoted SEARCH clauses:** an unquoted search clause contains one or more search words. If there is more than one word in the clause, then each word is logically AND’ed together. If two unquoted search clauses are adjacent with a statement, then a delimiter/separator is required between the two clauses. It can be either a semicolon or a plus-sign.

- ; [semi-colon]
- \+ [plus-sign]

**Quoted SEARCH clauses:** a quoted clause contains a single string of terms to search. An explicit match on the string is required. However, an ellipsis ( … ) can be used to indicate that wildcards may appear within the quoted string.

- It is called *quoted,* as the entire clause is sandwiched on both sides by double-quotes ( " )
- The absence of double-quotes means that the statement is unquoted

**Booleans and Negations:**

**and:** In Boolean logic, **and** means that all terms must be found. With Quelle, *and* is represented by terms that appear within an unquoted clause. 

**or:** In Boolean logic, **or** means that any term constitutes a match. With Quelle, *or* is represented by the semi-colon ( **;** ) or plus (+) between SEARCH clauses. 

**not:** In Boolean logic, means that the feature must not be found. With Quelle, *not* is represented by a minus,minus ( **--** ) and applies to an entire clause (it cannot be applied to individual segments (e.g. discrete words) within the search clause. However, a search clause is permitted to contain a single segment, which easily circumvents that limitation. In short, -- means subtract results; it cancels-out matches against all matches of other clauses. Most clauses are additive as each additional clause increases search results. Contrariwise, a **not** clause is subtractive as it decreases search results. Incidentally, -- was chosen over a single minus - so to allow for searches of hyphenated words without making such words ambiguous with negation.

Again, -- means that the clause will be subtracted from the search results.. When commands only contain a single search clause, it is always positive. A negative clause only makes sense when combined with another non-negative search clause as negative matches are subtracted from the search results. 

### Appendix B. Specialized Search tokens in Quelle (in Level-2 implementation)

Quelle-AVX, this includes all words in the original KJV text. It can optionally also search for modernized version of those words (e.g. hast and has; this is controllable with the %exact setting).  The table below lists additional linguistic extensions available in Quelle-AVX.

| Search Term  | Operator Type      | Meaning                                                      |
| ------------ | ------------------ | ------------------------------------------------------------ |
| fab\*        | wildcard           | starts with fab                                              |
| \*ous        | wildcard           | ends with ous                                                |
| fab\*ous     | wildcard           | starts with fab and ends with ous                            |
| \\is\\       | lemma              | search on all words that share the same lemma as is: be, is, are, were, etc |
| /noun/       | lexical marker     | any word where part of speech is a noun                      |
| /n/          | lexical marker     | synonym for /noun/                                           |
| /!n/         | lexical marker     | any word where part of speech is not a noun                  |
| /verb/       | lexical marker     | any word where part of speech is a verb                      |
| /v/          | lexical marker     | synonym for /verb/                                           |
| /!v/         | lexical marker     | any word where part of speech is not a verb                  |
| /pronoun/    | lexical marker     | any word where part of speech is a pronoun                   |
| /pn/         | lexical marker     | synonym for /pronoun/                                        |
| /!pn/        | lexical marker     | any word where part of speech is not a pronoun               |
| /adjective/  | lexical marker     | any word where part of speech is an adjective                |
| /adj/        | lexical marker     | synonym for /adjective/                                      |
| /!adj/       | lexical marker     | any word where part of speech is not an adjective            |
| /adverb/     | lexical marker     | any word where part of speech is an adverb                   |
| /adv/        | lexical marker     | synonym for /adverb/                                         |
| /!adv/       | lexical marker     | any word where part of speech is not an adverb               |
| /determiner/ | lexical marker     | any word where part of speech is a determiner                |
| /det/        | lexical marker     | synonym for /determiner/                                     |
| /!det/       | lexical marker     | any word where part of speech is not a determiner            |
| /1p/         | lexical marker     | any word where it is inflected for 1st person (pronouns and verbs) |
| /2p/         | lexical marker     | any word where it is inflected for 2nd person (pronouns and verbs) |
| /3p/         | lexical marker     | any word where it is inflected for 3rd person (pronouns, verbs, and nouns) |
| /singular/   | lexical marker     | any word that is known to be singular (pronouns, verbs, and nouns) |
| /plural/     | lexical marker     | any word that is known to be plural (pronouns, verbs, and nouns) |
| /WH/         | lexical marker     | any word that is a WH word (e.g., Who, What, When, Where, How) |
| /BoB/        | transition marker  | any word where it is the first word of the book (e.g. first word in Genesis) |
| /BoC/        | transition marker  | any word where it is the first word of the chapter           |
| /BoV/        | transition marker  | any word where it is the first word of the verse             |
| /EoB/        | transition marker  | any word where it is the last word of the book (e.g. last word in revelation) |
| /EoC/        | transition marker  | any word where it is the last word of the chapter            |
| /EoV/        | transition marker  | any word where it is the last word of the verse              |
| /!BoB/       | transition marker  | any word where it is not the first word of the book          |
| /!BoC/       | transition marker  | any word where it is not the first word of the chapter       |
| /!BoV/       | transition marker  | any word where it is not the first word of the verse         |
| /!EoB/       | transition marker  | any word where it is not the last word of the book           |
| /!EoC/       | transition marker  | any word where it is not the last word of the chapter        |
| /!EoV/       | transition marker  | any word where it is not the last word of the verse          |
| /Hsm/        | segment marker     | Hard Segment Marker (end) ... one of \. ? \!                 |
| /Csm/        | segment marker     | Core Segment Marker (end) ... :                              |
| /Rsm/        | segment marker     | Real Segment Marker (end) ... one of \. ? \! :               |
| /Ssm/        | segment marker     | Soft Segment Marker (end) ... one of ,\; \( \) --            |
| /sm/         | segment marker     | Any Segment Marker (end)  ... any of the above               |
| /_/          | punctuation        | any word that is immediately marked for clausal punctuation  |
| /!/          | punctuation        | any word that is immediately followed by an exclamation mark |
| /?/          | punctuation        | any word that is immediately followed by a question mark     |
| /./          | punctuation        | any word that is immediately followed by a period (declarative) |
| /-/          | punctuation        | any word that is immediately followed by a hyphen/dash       |
| /;/          | punctuation        | any word that is immediately followed by a semicolon         |
| /,/          | punctuation        | any word that is immediately followed by a comma             |
| /:/          | punctuation        | any word that is immediately followed by a colon (information follows) |
| /'/          | punctuation        | any word that is possessive, marked with an apostrophe       |
| /)/          | parenthetical text | any word that is immediately followed by a close parenthesis |
| /(/          | parenthetical text | any word contained within parenthesis                        |

### Appendix C. Object Model to example of Quelle (from the Blueprint-Blue implementation)

An object model to support specialized Search Tokens for Quelle-AVX is depicted below:

![QFind](C:\src\Quelle\QFind.png)



### Appendix D. Notional YAML for search interop (a search-oriented subset of Blueprint-Blue)

[EXAMPLE depicted as YAML; see Appendix F for FlatBuffer IDL definition]

```yaml
settings:
  exact: false
  span: 7,
  format: html

search:
  - find: time|help&/!verb/ ... need
    negate: false
    quoted: true
    - segment: time|help&/!verb/
      anchored: true
      - fragment: time|help
        - feature: time 
          wkeys: [ 1316 ]
        - feature: help
          wkeys: [ 795 ]
      - fragment: /!verb/
        - feature: /!verb/
          negate: true
          pos16: 0x100
    - segment: need
      anchored: false,
      - fragment: need
        - feature: need
          wkeys: [ 1026 ]

render: // if render is not specified, a summary of match results is returned as a FtatBuffer (see Appendix F)
    start: bcv // if v is not provided, it is implicitly verse 1 (book and chapter are required)
    count: 0xFF // verse-count: 0xFF implies the whole chapter
```
