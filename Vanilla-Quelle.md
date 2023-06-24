# Quelle Specification

##### version 2.0.3.612

### I. Background

Most modern search engines, provide a mechanism for searching via a text input box where the user is expected to type search terms. While primitive, this interface was pioneered by major web-search providers and represented an evolution from the far more complex interfaces that came earlier. When you search for multiple terms, however, there seems to be only one basic paradigm: “find every term”. The vast world of search is rife for a search-syntax that moves us past the ability to only search using basic search expressions. To this end, the Quelle specification is put forward an open Human-Machine-Interface (HMI) that can be invoked within a command shell or even within a simple text input box on a web page. The syntax fully supports basic Boolean operations such as AND, OR, and NOT. While great care has been taken to support the construction of complex queries, greater care has been taken to maintain a clear and concise syntax.

Quelle, IPA: [kɛl], in French means "What? or Which?". As Quelle HMI is designed to obtain search-results from search-engines, this interrogative nature befits its name. An earlier interpreter, Clarity, served as inspiration for defining Quelle.  You could think of the Quelle HMI as an evolution of Clarity HMI.  However, in order to create linguistic consistency in Quelle's Human-to-Machine command language, the resulting syntax varied so dramatically from the Clarity spec, that a new name was the best way forward.  Truly, Quelle HMI incorporates lessons learned after creating, implementing, and revising Clarity HMI for over a decade.

In 2023, Quelle 2.0 was published. This new specification is not a radical divergence from version 1. Most of the changes are centered around macros, control variables, filters, and export directives. Search syntax has remained largely unchanged. It turned out that the PEG grammar had some  ambiguity differentiating between the various implicit actions. To eliminate that ambiguity, new operators were introduced  [We added $ % and || to name a few] . These additions also reduce the need for clause delimiters. The version 2 spec feels more streamlined, more intuitive, and comes with a working revision of the PEG grammar. Implicit actions for Macros are now referred to as *apply* and *invoke* [those verbs replace *save* and *execute* respectively].

One fundamental change to the search specification in Quelle 2.0 is the dropping of support for bracketed search terms. While parsing these artifacts was easy within the PEG grammar, implementing the search from the parse was quite complex. That seldom-used feature doubled the complexity of corresponding search-algorithms. Having successfully implemented bracketed terms in the AV-Bible Windows application, I will make two strong assertions about bracketed terms:

1. implementation was intensely complex
2. almost no one used it.

For those two reasons, we have nixed bracketed terms from the grammar in the updated spec.

Every attempt has been made to make Quelle consistent with itself. Some constructs are in place to make parsing unambiguous, other constructs are biased toward ease of typing (such as limiting keystrokes that require the shift key). Of course, other command languages also influence our syntax, to make things more intuitive for a polyglot. In all, Quelle represents an easy to type and easy to learn HMI.  Moreover, simple search statements look no different than they might appear today in Google or Bing. Still, let's not get ahead of ourselves or even hint about where our simple specification might take us ;-)

### II. Addendum

A reference implementation of Quelle can be found in Quelle-AVX and related open source projects. There are two possible implementation levels:

- Level 1 [basic search support]
- Level 2 [search support includes also searching on part-of-speech tags]

### III. Quelle Syntax

Quelle defines a declarative syntax for specifying search criteria using the *find* verb. Quelle also defines additional verbs to round out its syntax as a simple straightforward means to interact with custom applications where searching text is the fundamental problem at hand.

Quelle Syntax comprises seventeen (17) verbs. Each verb corresponds to a basic action:

- find
- filter
- set
- get
- clear
- initialize
- export
- limit
- apply
- delete
- expand
- absorb
- help
- review
- invoke
- version
- exit

Quelle is an open and extensible standard, additional verbs, and verbs for other languages can be defined without altering the overall syntax structure of the Quelle HMI. The remainder of this document describes Version 1.0 of the Quelle-HMI specification.  

In Quelle terminology, a statement is made up of one or more clauses. Each clause represents an action. While there are seventeen action-verbs, there are only five syntax categories:

1. SEARCH
   - *find*
   - *filter*
2. CONTROL
   - *set*
   - *clear*
   - @get
   - @reset *(explicit alias for **clear all control settings**)*
   - @absorb *(use history **or** label/macro to **absorb all control settings**)*
3. OUTPUT
   - *limit*
   - *export*
4. SYSTEM
   - @help
   - @version
   - @exit
5. HISTORY & MACROS
   - *invoke*			  (invoke macro by its label **or** invoke a previous command by its id)
   - *apply*			   (apply label to macro)
   - @delete		  (delete macro by its label)
   - @expand		(label **or** id)
   - @review		 (review history)
   - @initialize	  (clear all history)

Each syntax category has either explicit and/or implicit actions.  Explicit actions begin with the @ symbol, immediately followed by the explicit verb.  Implicit actions are inferred by the syntax of the command.

### IV. Fundamental Quelle Commands

Learning just six verbs is all that is necessary to effectively use Quelle. In the table below, each verb is identified with required and optional parameters/operators.

| Verb      | Action Type | Syntax Category | Required Parameters       | Alternate/Optional Parameters |
| --------- | :---------: | :-------------- | ------------------------- | :---------------------------: |
| *find*    |  implicit   | SEARCH          | *search spec*             |                               |
| *filter*  |  implicit   | SEARCH          | **<** *domain*            |                               |
| *set*     |  implicit   | CONTROL         | **%name** **::** *value*  |    **%name** **=** *value*    |
| *limit*   |  implicit   | OUTPUT          | **[** *row_indices* **]** |                               |
| **@help** |  explicit   | SYSTEM          |                           |            *topic*            |
| **@exit** |  explicit   | SYSTEM          |                           |                               |

**TABLE 4-1** -- **Fundamental Quelle commands with corresponding syntax summaries**

From a linguistic standpoint, all Quelle commands are issued in the imperative. The subject of the verb is always "you understood". As the user, you are commanding Quelle what to do. Some verbs have direct objects [aka required parameters]. These parameters instruct Quelle <u>what</u> to act upon. The verb dictates the required parameters: in linguistic terms, this is referred to as the valence of the verb.

Quelle supports two types of actions:

1. Implicit actions [implicit actions are inferred from the syntax of their parameters]
2. Explicit actions [The verb needs to be explicitly stated in the command and it begins with **@**]

Implicit actions can be combined into compound statements.  However, compound statements are limited to contain ONLY implicit actions. This means that explicit actions cannot be used to construct a compound statement.

As search is a fundamental concern of Quelle, it is optimized to make compound implicit actions easy to construct with a concise and intuitive syntax. Even before we describe Quelle syntax generally, let's examine a few concepts using examples:

| Description                             | Example                                  |
| --------------------------------------- | :--------------------------------------- |
| SYSTEM command                          | @help                                    |
| SEARCH filters                          | < Wall-Street-Jornal < Washington-Post   |
| SEARCH specification                    | this is some text expected to be found   |
| Compound statement: two SEARCH actions  | "this quoted text" + other unquoted text |
| Compound statement: two CONTROL actions | %span = 7 %similarity = 85               |
| Compound statement: CONTROL & SEARCH    | %span = 7 Moses said                     |

**TABLE 4-2** -- **Examples of Quelle statement types**

### V. Deep Dive into Quelle SEARCH actions

Quelle syntax can alter the span by also supplying an additional CONTROL action:

*span=8 Harris Biden*

The statement above has a CONTROL action and a SEARCH action

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

span = 6 "/noun/ ... home run"

This would find phrases where a noun appeared within a span of six words and preceded "home run"

**Another SEARCH Example:**

Consider a query for all passages that contain a word beginning with pres, followed by Bush, but subtract phrases containing H W Bush.

*span = 15 ; "Pres** ... Bush" -- "H W Bush"*

*(this could be read as: find all references to a wildcard Pres [e.g. Pres. or President] Bush, but subtract phrases that also contain "HW Bush"*

**Valid statement syntax, but no results:**

this&that

/noun/ & /verb/

Both of the statements above are valid, but will not match any results. Search statements attempt to match actual words in the actual bible text. A bord cannot be "this" **and** "that". Likewise, an individual word in a sentence does not operate as a /noun/ **and** a /verb/. Contrariwise, these searches are valid, but would also return numerous matches:

this|that

/noun/ | /verb/

### VI. Displaying Results

Consider that there are two fundamental types of searches:

- Searches that return a limited set of results
- Searches that either return loads of results; or searches where the result count is unknown (and potentially very large)

Due to the latter condition above, SEARCH summarizes results (it does NOT display every result found). However, if more than a summary is desired, the user can control how many results to display.

"Clinton answered" *summarize documents that contain this phrase, with paragraph references*

"Clinton answered" [ ] *display every matching phrase* // equivalent to Clinton answered" ||[*]

"Clinton answered" [1] *this would would display only the first matching phrase*

"Clinton answered" [1 2 3] *this would would display only the first three matching phrases*

"Clinton answered" [4 5 6] *this would would display the next first three matching phrases*

### VII. Exporting Results

Export using a display-coordinate:

To revisit the example in the previous sample, we can export records to a file with these commands:

"Clinton answered" [ ] > my-file.output // *this would export every matching phrase*

"Clinton answered" [1] > my-file.output // *this would would export only the first matching phrase*

"Clinton answered" [1 2 3] >> my-file.output // *this would would export the first three matching phrases* // >> indicates that the results should be appended

format=html ; "Clinton answered" [1 2 3] => my-file.html // *export the first three matching phrases as html*

The => allows existing file to be overwritten. Quelle will not overwrite an existing file with > syntax. The => is required to force an overwrite.

| Verb     | Action Type | Syntax Category | Parameters       | Alternate #1      | Alternate #2      |
| -------- | :---------: | --------------- | ---------------- | :---------------- | :---------------- |
| *export* |  implicit   | OUTPUT          | **>** *filename* | **=>** *filename* | **>>** *filename* |

**TABLE 7-1** -- **The implicit export action**

### VIII. Filtering Results

Sometimes we want to constrain the domain of where we are searching. Say that I want to find mentions of the Joe Biden in the Wall Street Journal. I can search only WSJ by executing this search:

"Joe Biden" < WSJ

If I also want to search in Wall Street Journal and the New York Post, this works:

serpent < WSJ < NYP

Search domains and associated abbreviations must be explicitly supported by the Quelle Driver. The domain is captured by the Pinshot-blue parser, but the object-model needs explicit adaptation to constrain searches by domain designations.

### IX. Labeling & Reviewing Statements for subsequent invocation

| Verb            | Action Type | Syntax Category           | Expected Arguments        | Required Operators |
| --------------- | ----------- | ------------------------- | ------------------------- | :----------------: |
| *invoke*        | implicit    | HISTORY & LABELING        | *label* or *id*           |   **$** *label*    |
| *apply*         | implicit    | HISTORY & <u>LABELING</u> | *label*                   |  **\|\|** *label*  |
| **@delete**     | explicit    | HISTORY & <u>LABELING</u> | *label*                   |      *label*       |
| **@expand**     | explicit    | HISTORY & LABELING        | *label* or *id*           |  *label* or *id*   |
| **@absorb**     | explicit    | CONTROL                   | *label* or *id*           |  *label* or *id*   |
| **@review**     | explicit    | <u>HISTORY</u> & LABELING | **optional:** *max_count* |                    |
| **@initialize** | explicit    | <u>HISTORY</u> & LABELING |                           |                    |

**TABLE 9-1** -- **Labeling statements and reviewing statement history**

In this section, we will examine how user-defined macros are used in Quelle. A macro in Quelle is a way for the user to label a statement for subsequent use. By applying a label to a statement, a shorthand mechanism is created for subsequent utilization. This gives rise to two new definitions:

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

%span::7 %exact::true + eternal power + diety

To illustrate this further, here are four more examples of labeled statement definitions:

%exact::1 || C1

%span::8 || C2

+nation || F1

+eternal || F2

We can utilize these as a compound statement by issuing this command:

$C1 $C2 $F1 $F2

Similarly, we could define another label from these, by issuing this command:

$C1 $C2 $F1 $F2 || another-macro

This expands to:

%exact::1 %span::8 nation + eternal

There are several restrictions on macro definitions:

1. Macro definition must represent a valid Quelle statement:
   - The syntax is verified prior to applying the statement label.
2. Macro definitions exclude export directives
   - Any portion of the statement that contains > is incompatible with a macro definition
3. Macro definitions exclude output directives
   - Any portion of the statement that contains [ ] is incompatible with a macro definition
4. The statement cannot represent an explicit action:
   - Only implicit actions are permitted in a labelled statement.

Finally, any macros referenced within a macro definition are expanded prior to applying the new label. Therefore redefining a macro after it has been referenced in a subsequent macro definition has no effect of the initial macro reference. We call this macro-determinism. A component of determinism for macros is that the macro definition saves all control settings at the time that the label was applied. This assure that the same search results are returned each time the macro is referenced. However, if the user desires the current settings to be used instead, just ***::current*** suffix after the macro.

##### Additional explicit macro commands:

Two additional explicit commands exist whereby a macro can be manipulated. We saw above how they can be defined and referenced. There are two additional ways commands that operate on macros: expansion and deletion. In the last macro definition above where we created $another-macro, the user could preview an expansion by issuing this command:

@expand another-macro

If the user wanted to remove this definition, the @delete action is used. Here is an example:

@delete another-macro

NOTE: Labels must begin with a letter [A-Z] or [a-z], but they may contain numbers, spaces, hyphens, periods, commas, underscores, and single-quotes (no other punctuation or special characters are supported).

While macro definitions are deterministic, they can be overwritten/redefined.

**REVIEW SEARCH HISTORY**

*@review* gets the user's search activity for the current session.  To show the last ten searches, type:

*@review*

To show the last three searches, type:

*@review* 3

To show the last twenty searches, type:

*@review* 20 

**INVOKE**

The *invoke* command works for command-history works exactly the same way as it does for macros.  After issuing a *@review* command, the user might receive a response as follows.

*@review*

1>  %span = 7

2>  %similarity::85

3> eternal power

And the invoke command can re-invoke any command listed.

$3

would be shorthand to re-invoke the search specified as:

eternal power

Again, *invoking* command from your command history is *invoked* just like a macro. Moreover, as with macros, control settings are persisted within your command history to provide invocation determinism. That means that the current control settings are ignored when invoking command history. Just like with macros, the current control settings can be utilized by adding the ***::current*** suffix to the invocation. See **Table 12-5**. Example usage:

$3::current

or we could re-invoke all three commands in a single statement as:

$1  $2  $3

which would be interpreted as:

%span = 7  %similarity::85   eternal power

**RESETTING COMMAND HISTORY**

The @reset command can be used to clear command history and/or reset all control variables to defaults.

To clear all command history:

@initialize history

##### Invoking a command remembers all settings, but always without side-effects:

Command history captures all settings. We have already discussed macro-determinism. Invoking commands by their review numbers behave exactly like macros. They are also deterministic. Just like macros, an setting with an equals sign (e.g. span=7) is treated as if it were (span::7). In other words, invoking command history never persists changes into your environment, unless you explicitly request such behavior with the @absorb command.

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

### XII. Control Settings & additional related cpmmands

| Verb     | Action Type | Syntax Category | Parameters             | Alternate #1          | Alternate #2 |
| -------- | :---------: | --------------- | ---------------------- | :-------------------- | :----------- |
| *clear*  |  implicit   | CONTROL         | *%name* **:: default** | *%name* **= default** |              |
| **@get** |  explicit   | CONTROL         | **optional:** *name*   |                       |              |

**TABLE 12-1** -- **Listing of additional CONTROL actions**



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

Vanilla-Quelle manifests just two control names. Both allow all three actions: ***set***, ***clear***, and ***@get*** verbs. Table 12-4 lists both settings available in Vanilla-Quelle. 

| Setting | Meaning                     | Values            | Default Value |
| ------- | --------------------------- | ----------------- | ------------- |
| span    | proximity distance limit    | 0 to 999 or verse | 0 [verse]     |
| format  | format of results on output | see Table 12-2    | json          |

**TABLE 12-4** -- **Summary of Vanilla-Quelle Control Names**

The *@get* command fetches these values. The *@get* command requires a single argument. Examples are below:

*@get* search

@get format

There are additional actions that affect all control settings collectively

| Expressions | Meaning / Usage                                              |
| ----------- | ------------------------------------------------------------ |
| **@reset**  | Reset is an explicit command alias to *clear* all control settings, resetting them all to default values<br />equivalent to: %span=default %lexicon=default %display=default %similarity=default %format=default |
| $X::current | Special suffix for use with History or Macro invocation as a singleton statement:<br />See "Labeling Statements for subsequent invocation" section of this document.<br />Uses current settings for invocation on history/macro identified/labeled as "X".<br>(On non-singleton invocations, environment settings on the macro/history is **always** ignored, making the ::current suffix superfluous on compound macro satements) |

**TABLE 12-5** -- **Collective CONTROL operations**

All settings can be cleared using an explicit command:

@reset

It is exactly equivalent to this compound statement:

%span=default  %format=default

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

---

In general, Quelle can be thought of as a stateless server. The only exceptions of its stateless nature are:

1) non-default settings with persistent scope
2) defined macro labels. 
3) command history

Finally delimiters ( e.g.  ; or + ), between two distinct unquoted search clauses are ***required*** to identify the boundary between the two search clauses. A single delimiter <u>before</u> any search clause is always ***optional*** (e.g., Delimiters are permitted between quoted and unquoted clauses, and any other clause that precedes any search clause). They are ***unexpected*** anywhere else in the statement. This makes a Quelle statement a bit less cluttered than the version 1.0 specification

---

An object model to manifest the Quelle grammar is depicted below:

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

**not:** In Boolean logic, means that the feature must not be found. With Quelle, *not* is represented by a hyphen+colon ( **-:** ) and applies to individual features within a search segment within the search clause. It is best used in conjunction with other features, because any non-match will be included in results. 

hyphen+colon ( **-:** ) means that any non-match satisfies the search condition. Used by itself, it would likely return every verse. Therefore, it should be used judiciously.

### Appendix B. Specialized Search tokens in a Quelle Level-II driver

The table below lists additional linguistic extensions available in a Level-II Quelle implementation.

| Search Term  | Operator Type      | Meaning                                                      | Maps To               | Mask     |
| ------------ | ------------------ | ------------------------------------------------------------ | --------------------- | -------- |
| Jer\*        | wildcard           | starts with Jer                                              | selects from lexicon  | 0x3FFF   |
| \*iah        | wildcard           | ends with iah                                                | selects from lexicon  | 0x3FFF   |
| Jer\*iah     | wildcard           | starts with Jer and ends with iah                            | Jer\* & \*iah         | as above |
| \\is\\       | lemma              | search on all words that share the same lemma as is: be, is, are, art, etc | be\|is\|are\|art\|... | 0x3FFF   |
| /noun/       | lexical marker     | any word where part of speech is a noun                      | POS12::0x010          | 0x0FF0   |
| /n/          | lexical marker     | synonym for /noun/                                           | POS12::0x010          | 0x0FF0   |
| /verb/       | lexical marker     | any word where part of speech is a verb                      | POS12::0x100          | 0x0FF0   |
| /v/          | lexical marker     | synonym for /verb/                                           | POS12::0x100          | 0x0FF0   |
| /pronoun/    | lexical marker     | any word where part of speech is a pronoun                   | POS12::0x020          | 0x0FF0   |
| /pn/         | lexical marker     | synonym for /pronoun/                                        | POS12::0x020          | 0x0FF0   |
| /adjective/  | lexical marker     | any word where part of speech is an adjective                | POS12::0xF00          | 0x0FFF   |
| /adj/        | lexical marker     | synonym for /adjective/                                      | POS12::0xF00          | 0x0FFF   |
| /adverb/     | lexical marker     | any word where part of speech is an adverb                   | POS12::0xA00          | 0x0FFF   |
| /adv/        | lexical marker     | synonym for /adverb/                                         | POS12::0xA00          | 0x0FFF   |
| /determiner/ | lexical marker     | any word where part of speech is a determiner                | POS12::0xD00          | 0x0FF0   |
| /det/        | lexical marker     | synonym for /determiner/                                     | POS12::0xD00          | 0x0FF0   |
| /1p/         | lexical marker     | any word where it is inflected for 1st person (pronouns and verbs) | POS12::0x100          | 0x3000   |
| /2p/         | lexical marker     | any word where it is inflected for 2nd person (pronouns and verbs) | POS12::0x200          | 0x3000   |
| /3p/         | lexical marker     | any word where it is inflected for 3rd person (pronouns, verbs, and nouns) | POS12::0x300          | 0x3000   |
| /singular/   | lexical marker     | any word that is known to be singular (pronouns, verbs, and nouns) | POS12::0x400          | 0xC000   |
| /plural/     | lexical marker     | any word that is known to be plural (pronouns, verbs, and nouns) | POS12::0x800          | 0xC000   |
| /WH/         | lexical marker     | any word that is a WH word (e.g., Who, What, When, Where, How) | POS12::0xC00          | 0xC000   |
| /_/          | punctuation        | any word that is immediately marked for clausal punctuation  | PUNC::!=0x00          | 0xE0     |
| /!/          | punctuation        | any word that is immediately followed by an exclamation mark | PUNC::0x80            | 0xE0     |
| /?/          | punctuation        | any word that is immediately followed by a question mark     | PUNC::0xC0            | 0xE0     |
| /./          | punctuation        | any word that is immediately followed by a period (declarative) | PUNC::0xE0            | 0xE0     |
| /-/          | punctuation        | any word that is immediately followed by a hyphen/dash       | PUNC::0xA0            | 0xE0     |
| /;/          | punctuation        | any word that is immediately followed by a semicolon         | PUNC::0x20            | 0xE0     |
| /,/          | punctuation        | any word that is immediately followed by a comma             | PUNC::0x40            | 0xE0     |
| /:/          | punctuation        | any word that is immediately followed by a colon (information follows) | PUNC::0x60            | 0xE0     |
| /'/          | punctuation        | any word that is possessive, marked with an apostrophe       | PUNC::0x10            | 0x10     |
| /)/          | parenthetical text | any word that is immediately followed by a close parenthesis | PUNC::0x0C            | 0x0C     |
| /(/          | parenthetical text | any word contained within parenthesis                        | PUNC::0x04            | 0x04     |

### Appendix C. Object Model that supports search tokens in Quelle-AVX

An object model to support specialized Search Tokens for Quelle-AVX (a superset of Vanilla Quelle) is depicted below:

![QFind](C:\src\Quelle\QFind.png)


