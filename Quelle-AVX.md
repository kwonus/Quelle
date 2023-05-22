# Quelle-AVX Specification

##### version 2.0.3.521

### I. Background

Most modern search engines, provide a mechanism for searching via a text input box where the user is expected to type search terms. While primitive, this interface was pioneered by major web-search providers and represented an evolution from the far more complex interfaces that came earlier. When you search for multiple terms, however, there seems to be only one basic paradigm: “find every term”. The vast world of search is rife for a search-syntax that moves us past the ability to only search using basic search expressions. To this end, the Quelle specification is put forward an open Human-Machine-Interface (HMI) that can be invoked within a command shell or even within a simple text input box on a web page. The syntax fully supports basic Boolean operations such as AND, OR, and NOT. While great care has been taken to support the construction of complex queries, greater care has been taken to maintain a clear and concise syntax.

Quelle, IPA: [kɛl], in French means "What? or Which?". As Quelle HMI is designed to obtain search-results from search-engines, this interrogative nature befits its name. An earlier interpreter, Clarity, served as inspiration for defining Quelle.  You could think of the Quelle HMI as an evolution of Clarity HMI.  However, in order to create linguistic consistency in Quelle's Human-to-Machine command language, the resulting syntax varied so dramatically from the Clarity spec, that a new name was the best way forward.  Truly, Quelle HMI incorporates lessons learned after creating, implementing, and revising Clarity HMI for over a decade.

In 2023, Quelle-AVX 2.0 was released. This new release is not a radical divergence from version 1. Most of the changes are centered around macros, control variables, filters, and export directives. Search syntax has remained almost entirely unchanged. It turned out that the PEG grammar had some  ambiguity differentiating between the various implicit actions. To eliminate that ambiguity, new operators were introduced  [We added $ % and || to name a few] . These additions also reduced the need for clause delimiters. The version 2 spec feels more streamlined, more intuitive, and comes with a working revision of the PEG grammar. Implicit actions for Macros have been reclassified as *apply* and *utilize* [those verbs replace *save* and *execute* respectively] .  These new verbs align with the metaphor of labelling.  As Quelle-AVX is the only current known reference implementation of Quelle, it is likely that Vanilla-Quelle will eventually undergo this same evolution.

The one change to the search specification in Quelle 2.0 is the dropping of support for bracketed search terms. While parsing these artifacts was easy with the PEG grammar, implementing the search from the parse was quite complex. That seldom-used feature doubled the complexity of corresponding search-algorithms. Having successfully implemented bracketed terms in the AV-Bible Windows application, I make make two strong assessments:

1. implementation was intensely complex
2. almost no one used it.

For those two reasons, we have nixed the feature from the updated spec. An upcoming feature, that provides some overlapping functionality. is discussed in more detail in the ***Filtering Results*** section of this document.

Every attempt has been made to make Quelle consistent with itself. Some constructs are in place to make parsing unambiguous, other constructs are biased toward ease of typing (such as limiting keystrokes that require the shift key). Of course, other command languages also influence our syntax, to make things more intuitive for a polyglot. In all, Quelle represents an easy to type and easy to learn HMI.  Moreover, simple search statements look no different than they might appear today in a Google or Bing search box. Still, let's not get ahead of ourselves or even hint about where our simple specification might take us ;-)

### II. Addendum

Quelle-AVX extends baseline Vanilla-Quelle to include AVX-specific constructs.
Each section below identifies specialized constructs for parsing AVX commands using the Quelle parser.

Vanilla Quelle specifies two possible implementation levels:

- Level 1 [basic search support]
- Level 2 [search support includes also searching on part-of-speech tags]

AVX-Quelle is Level 2 implementation with additional specialized search capabilities. However, there are two features of Vanilla-Quelle where AVX-Quelle diverges from the baseline specification.

1.  AVX-Quelle drops support for polarity on search clauses. Vanilla-Quelle uses minus+minus ( -- ) to negate entire search clauses. AVX-Quelle does not support that operation. Instead, AVX-Quelle allows individual features within a search clause to be negated using Minus+colon ( -: )
2. AVX-Quelle does not support the %exact setting of Vanilla-Quelle. Instead, it offers two thresholds: %threashold.text and %threshold.phonics. A setting of 100 (i.e. 100%) behaves much like %exact = true of Vanilla-Quelle. Yet, AVX-Quelle allows the specification of fuzziness thresholds using these enhanced settings. This in conjunction with the lexicon setting allow greater control over matching logic than Vanilla-Quelle.

### III. Quelle Syntax

Just like the baseline Quelle specification, Quelle-AVX defines a declarative syntax for specifying search criteria using the *find* verb. Quelle also defines additional verbs to round out its syntax as a simple straightforward means to interact with custom applications where searching text is the fundamental problem at hand.

Quelle Syntax comprises eighteen(18) verbs. Each verb corresponds to a basic action:

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

In Quelle terminology, a statement is made up of one or more clauses. Each clause represents an action. While there are eighteen action-verbs, there are only six syntax categories:

1. SEARCH
   - *find*
   - *filter*
2. CONTROL
   - *set*
   - *clear*
   - @get
   - @reset *(explicit alias for "**clear all control settings**")*
4. OUTPUT
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
   - @absorb

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
| Compound statement: two CONTROL actions | %span = 7 %threshold.text = 85           |
| Compound statement: CONTROL & SEARCH    | %span = 7 Moses said                     |

**TABLE 4-2** -- **Examples of Quelle statement types**

Consider these two examples of Quelle statements (first CONTROL; then SEARCH):

%lexicon = KJV

"Moses"

Notice that both statements above are single actions.  We should have a way to express both of these in a single command. And this is the rationale behind a compound statement. To combine the previous two actions into one compound statement, issue this command:

"Moses" + lexicon=KJV

### V. Deep Dive into Quelle SEARCH actions

Consider this proximity search where the search using Quelle syntax:

*lexicon=KJV  Moses*

Quelle syntax can alter the span by also supplying an additional CONTROL action:

*lexicon=KJV  Moses*

The statement above has two CONTROL actions and one SEARCH action

Next, consider a search to find Moses or Aaron:

*Moses|Aaron*

The order in which the search terms are provided is insignificant. Additionally, the type-case is insignificant. 

Of course, there are times when word order is significant. Accordingly, searching for explicit strings can be accomplished using double-quotes as follows:

*"Moses said ... Aaron"*

These constructs can even be combined. For example:

*"Moses said ... Aaron|Miriam"*

The search criteria above is equivalent to this search:

*"Moses said ... Aaron" + "Moses said ... Miriam"*

In all cases, “...” means “followed by”, but the ellipsis allows other words to appear between "said" and "Aaron". Likewise, it allows words to appear between "said" and "Miriam". 

Quelle is designed to be intuitive. It provides the ability to invoke Boolean logic for term-matching linguistic feature-matching. As we saw above, the pipe symbol ( | ) can be used to invoke an *OR* condition In effect, this invokes Boolean multiplication on the terms and features that compose the expression.

The ampersand symbol can similarly be used to represent *AND* conditions upon terms. As an example. the English language contains words that can sometimes as a noun , and other times as a noun or other part-of-speech. To determine if the bible text contains the word "part" where it is used as a verb, we can issue this command:

"part&/noun/"

The SDK, provided by Digital-AV, has marked each word of the bible text for part-of-speech. With Quelle's rich syntax, make this type of search easy and intuitive.

Of course, part-of-speech expressions can also be used independently of the an AND condition, as follows:

%span = 6 + "/noun/ ... home"

That search would find phrases where a noun appeared within a span of six words, preceding the word "home"

**Valid statement syntax, but no results:**

this&that

/noun/ & /verb/

**Negating search-terms Example:**

Consider a query for all passages that contain a word beginning, followed by any word that is neither a verb nor an adverb:

%span = 15 "Lord\* -:/v/ & -:/adv/

Both of the statements above are valid, but will not match any results. Search statements attempt to match actual words in  the actual bible text. A bord cannot be "this" **and** "that". Likewise, an individual word in a sentence does not operate as a /noun/ **and** a /verb/. Contrariwise, these searches are valid, but would also return numerous matches:

this|that

/noun/ | /verb/

### VI. Displaying Results

Consider that there are two fundamental types of searches:

- Searches that return a limited set of results
- Searches that either return loads of results; or searches where the result count is unknown (and potentially very large)

Due to the latter condition above, SEARCH summarizes results (it does NOT display every result found). However, if more than a summary is desired, the user can control how many results to display.

"Jesus answered"			*summarize documents that contain this phrase, with paragraph references*

"Jesus answered" [ ]        *display every matching phrase* // equivalent to Jesus answered"

"Jesus answered" [1]  *this would would display only the first matching phrase*

"Jesus answered" [1 2 3]  *this would would display only the first three matching phrases*

"Jesus answered" [4 5 6]  *this would would display the next three matching phrases*

### VII. Exporting Results

Export using a display-coordinate:

To revisit the example in the previous sample, we can export records to a file with these commands:

"Jesus answered" [ ] > my-file.output  // *this would export every matching phrase*

"Jesus answered" [1]  > my-file.output  // *this would would export only the first matching phrase*

"Jesus answered" [1 2 3]  >> my-file.output  //  *this would would export the first three matching phrases* // >> indicates that the results should be appended

format=html ; "Jesus answered" [1 2 3]  => my-file.html // *export the first three matching phrases as html*

The => allows existing file to be overwritten. Quelle will not overwrite an existing file with > syntax. The => is required to force an overwrite.

### VIII. Filtering Results

Sometimes we want to constrain the domain of where we are searching. Say that I want to find mentions of the serpent in Genesis. I can search only Genesis by executing this search:

serpent < Genesis

If I also want to search in Genesis and Revelation, this works:

serpent < Genesis < Revelation

Filters do not allow spaces, but they do allow Chapter and Verse specifications. To search for the serpent in Genesis Chapter 3, we can do this:

serpent < Genesis:3

And books that contain spaces are supported by eliminating the spaces. For example, this is a valid command:

vanity < SongOfSolomon < 1Corinthians

Abbreviations are also supported:

vanity < sos < 1co

### IX. Labelling Statements for subsequent utilization

| Verb        | Action Type | Syntax Category  | Required Arguments     | Required Operators |
| ----------- | ----------- | ---------------- | ---------------------- | :----------------: |
| *apply*     | implicit    | LABEL            | **1**: *label*         |  **\|\|** *label*  |
| **@delete** | explicit    | LABEL            | **1**: *label*         |      *label*       |
| **@expand** | explicit    | LABEL or HISTORY | **1**: *label* or *id* |  *label* or *id*   |
| **@absorb** | explicit    | LABEL or HISTORY | **1**: *label* or *id* |  *label* or *id*   |
| *utilize*   | implicit    | LABEL            | **1+**: *labels*       |   **$** *label*    |

**TABLE 9-1** -- **Utilizing labelled statements and related commands**

In this section, we will examine how user-defined macros are used in Quelle.  A macro in Quelle is a way for the user to label a statement for subsequent use.  By applying a label to a statement, a shorthand mechanism is created for subsequent utilization. This gives rise to two new definitions:

1. Labelling a statement (or defining a macro; labels must begin with a letter; never a number, underscore, or hyphen)

2. Utilization of a labelled statement (running a macro)


Let’s say we want to name the search example from the previous section; We’ll call it *eternal-power*. To accomplish this, we would issue this command:

%span::7 %threshold.text::85 + eternal power || eternal-power

It’s that simple, now instead of typing the entire statement, we can utilize the macro by referencing our previously applied label. Here is how the macro can be utilized. We might call this running the macro:

$eternal-power

Labelled statements also support compounding using the semi-colon ( ; ), as follows; we will label it also:

$eternal-power + godhead|| my-label-cannot-contain-spaces

Later I can issue this command:

$my-label-cannot-contain-spaces

Which is equivalent to these statements:

%span::7  %threshold.text::85 + eternal power + godhead

To illustrate this further, here are four more examples of labeled statement definitions:

%threshold.text::85 || C1

%span::8  || C2

+nation || F1

+eternal  || F2

We can utilize these as a compound statement by issuing this command:

$C1  $C2  $F1  $F2

Similarly, we could define another label from these, by issuing this command:

$C1 $C2  $F1  $F2 || another-macro

This expands to:

%threshold.text::85  %span::8   nation + eternal

There are several restrictions on macro definitions:

1. Macro definition must represent a valid Quelle statement:
   - The syntax is verified prior to applying the statement label.
2. Macro definitions exclude export directives
   - Any portion of the statement that contains > is incompatible with a macro definition
3. Macro definitions exclude output directives
   - Any portion of the statement that contains [ ] is incompatible with a macro definition
4. The statement cannot represent an explicit action:
   - Only implicit actions are permitted in a labelled statement.

Finally, any macros referenced within a macro definition are expanded prior to applying the new label. Therefore redefining a macro after it has been referenced in a subsequent macro definition has no effect of the initial macro reference. We call this macro-determinism.  A component of determinism for macros is that the macro definition saves all control settings at the time that the label was applied. This assure that the same search results are returned each time the macro is referenced. Here is an example.

%span = 2

in beginning || in_beginning

%span = 3

$in_beginning [1] < genesis:1:1

***result:*** none

However, if the user desires the current settings to be used instead, just include ***::current*** suffix after the macro. 

$in_beginning::current [1] < genesis:1:1

***result:*** Gen 1:1 In the beginning, God created ...

It should be noted that when a macro is paired with any other search clauses, it implicitly adds the settings::current suffix to all macros. Incidentally, the @expand command for history and macros reveals which settings are bundled in the history or macro.

##### Executing a macro remembers all settings, but always without side-effects:

A macro definition captures all settings. We have already discussed macro-determinism (saving settings utilized for execution is needed to provide macro determinism). However, an setting with an equals sign (e.g. span=7) is treated as if it were (span::7). In other words, executing a macro never persists changes into your environment, unless you explicitly request such behavior with the @absorb command.

##### Additional explicit macro commands:

Two additional explicit commands exist whereby a macro can be manipulated. We saw above how they can be defined and referenced. There are two additional ways commands that operate on macros: expansion and deletion.  In the last macro definition above where we created  $another-macro, the user could preview an expansion by issuing this command:

@expand another-macro

If the user wanted to remove this definition, the @delete action is used.  Here is an example:

@delete another-macro

If you want the same settings to be persisted to your current session that were in place during macro definition, the @absorb command will persist all settings for the macro into your current session

@absorb my-favorite-settings-macro 

Both @absorb and @expand also work with command history.

**NOTE:** Labels must begin with a letter [A-Z] or [a-z], but they may contain numbers, spaces, hyphens, periods, commas, underscores, and single-quotes (no other punctuation or special characters are supported).

While macro definitions are deterministic, they can be overwritten/redefined: consider this sequence:

"Jesus said" || jesus_macro

"Mary said" || other_macro

$Jesus_macro  $other_macro || either_said

@expand either_said

***result:***	"Jesus said"   "Mary said"

"Peter said" || other_macro

@expand either_said

***result:***	"Jesus said"   "Mary said"

$Jesus_macro   $other_macro || either_said

@expand either_said

***result:***	"Jesus said"   "Peter said"

The sequence above illustrates both macro-determinism and the ability to explicitly redefine a macro.

### X. Reviewing Statement History and re-invoking statements

| Verb            | Action Type | Required Parameter | Optional Parameters |
| --------------- | ----------- | :----------------: | :-----------------: |
| *invoke*        | implicit    |     **$** *id*     |                     |
| **@review**     | explicit    |                    |     *max_coun*t     |
| **@initialize** | explicit    |      history       |                     |

##### **TABLE 10-1 -- Reviewing history and re-invoking previous commands**

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

2>  %threshold.text::85

3> eternal power

And the invoke command can re-invoke any command listed.

$3

would be shorthand to re-invoke the search specified as:

eternal power

*Invoking* command history is very much analogous with *utilizing* a macro. Just like a macro, the control settings are saved to provide determinism. That means that the current control settings are ignored when invoking command history. Just like with macros, the current control settings can be utilized by adding the ***::current*** suffix to the invocation. See **Table 13-5**. Example usage:

$3::current

or we could re-invoke all three commands in a single statement as:

$1  $2  $3

which would be interpreted as:

%span = 7  %threshold.text::85   eternal power

**RESETTING COMMAND HISTORY**

The @reset command can be used to clear command history and/or reset all control variables to defaults.

To clear all command history:

@initialize history

##### Invoking a command remembers all settings, but always without side-effects:

Command history captures all settings. We have already discussed macro-determinism. Invoking commands by their review numbers behave exactly like macros. They are also deterministic. Just like macros, an setting with an equals sign (e.g. span=7) is treated as if it were (span::7). In other words, invoking command history never persists changes into your environment, unless you explicitly request such behavior with the @absorb command.

### XI. Program Help

*@help*

This will provide a help message in a Quelle interpreter.

Or for specific topics:

*@help* find

*@help* set

@help export

etc ...

### XII. Exiting Quelle

Type this to terminate the Quelle interpreter:

*@exit*

### XIII. Control Settings

| Verb     | Action Type | Syntax Category | Parameters             | Alternate #1          | Alternate #2      |
| -------- | :---------: | --------------- | ---------------------- | :-------------------- | :---------------- |
| *clear*  |  implicit   | CONTROL         | *%name* **:: default** | *%name* **= default** |                   |
| *export* |  implicit   | OUTPUT          | **>** *filename*       | **=>** *filename*     | **>>** *filename* |
| **@get** |  explicit   | CONTROL         | *name*                 |                       |                   |

**TABLE 13-1** -- **Listing of additional CONTROL, OUTPUT & SYSTEM actions**

**Export Format Options:**

| **Markdown**   | **HTML**         | **Text**         | Json             |
| -------------- | ---------------- | ---------------- | ---------------- |
| *%format = md* | *%format = html* | *%format = text* | *%format = json* |

**TABLE 13-2** -- **set** format command can be used to set the default content-formatting for for use with the export verb



| **example**    | **explanation**                                              |
| -------------- | ------------------------------------------------------------ |
| span = 8       | Assign a control setting                                     |
| **@get** span  | get a control setting                                        |
| span = default | Clear the control setting; restoring the Quelle driver default setting |

**TABLE 13-3** -- **set/clear/get** action operate on configuration settings



**SETTINGS:**

Otherwise, when multiple clauses contain the same setting, only the last setting in the list is preserved.  Example:

format=md   format=default  format=text

@get format

The final command would return text.  We call this: "last assignment wins". However, there is one caveat to this precedence order: regardless of where in the statement a macro or history invocation is provided within a statement, it never has precedence over a setting that is actually visible within the statement.  Consider this sequence as an example:

%threshold.text::none || precedence_example

%threshold.text::85  $precedence_example

@get %threshold.text

The final command would return 85, because it was visible in the compound statement.

Quelle-AVX manifests four control names. Each allows all three actions: ***set***, ***clear***, and ***@get*** verbs. Table 13-4 lists all settings available in AVX-Quelle. Quelle does not support the *exact* setting found in Vanilla Quelle. As AVX-Quelle can support two distinct orthographies [i.e. Contemporary Modern English (avx/modern), and Early Modern English (avx/modern)], the binary setting found in Vanilla-Quelle was not a sufficient designation.

| Setting           | Meaning                                                      | Values                                                       | Default Value |
| ----------------- | ------------------------------------------------------------ | ------------------------------------------------------------ | ------------- |
| span              | proximity distance limit                                     | 0 to 999 or verse                                            | 0 [verse]     |
| lexicon           | the lexicon to be used for the searching                     | av/avx/dual (kjv/modern/both)                                | dual (both)   |
| display           | the lexicon to be used for display/rendering                 | av/avx (kjv/modern)                                          | av (kjv)      |
| format            | format of results on output                                  | see Table 13-2                                               | json          |
| threshold.text    | fuzzy text matching threshold is a percentage between 33 and 99<br>0 or *none* means: do not match on text (use phonics only)<br>100 or *exact* means that an *exact* text match is expected | 33 to 99 [fuzzy] **or** ...<br>0 **or** *none*<br>100 **or** *exact* | 100 (exact)   |
| threshold.phonics | fuzzy phonetics matching threshold is between 33 and 99<br/>0 or *none* means: do not match on phonetics (use text only)<br/>100 or *exact* means that an *exact* phonetics match is expected | 33 to 99 [fuzzy] **or** ...<br>0 **or** *none*<br>100 **or** *exact* | 0 (none)      |
| ~~exact~~         | ~~exact vs approximate match~~ [replaced by thresholds & lexicon settings] | ~~true/false~~                                               | ~~false~~     |

**TABLE 13-4** -- **Summary of Quelle-AVX Control Names**

The *@get* command fetches these values. The *@get* command requires a single argument. Examples are below:

*@get* search

@get format

There are additional actions that affect all control settings collectively

| Expressions | Meaning / Usage                                              |
| ----------- | ------------------------------------------------------------ |
| **@reset**  | Reset is an explicit command alias to *clear* all control settings, resetting them all to default values<br />equivalent to: %span=default %lexicon=default %display=default %threshold.text=default %threshold.phonics=default %format=default |
| $X::current | Special suffix for use with History or Macro executed as a singleton statement:<br />See "Labelling Statements for subsequent utilization" section of this document.<br />Uses current settings for invocation/utilization on history/macro identified/labelled as "X". |

**TABLE 13-5** -- **Collective CONTROL operations**

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

### XIV. Miscellaneous Information

| Verb         | Action Type | Syntax Category |
| ------------ | :---------: | --------------- |
| **@version** |  explicit   | SYSTEM          |

**QUERYING DRIVER FOR VERSION INFORMATION**

@version will query the Quelle Search Provider for version information:

---

In general, AVX-Quelle can be thought of as a stateless server. The only exceptions of its stateless nature are:

1) non-default settings with persistent scope
2) defined macro labels. 
3) command history

Finally delimiters ( e.g.  ; or + ), between two distinct unquoted search clauses are ***required*** to identify the boundary between the two search clauses. A single delimiters <u>before</u> any search clause is always ***optional*** (e.g., Delimiters are permitted between quoted and unquoted clauses, and any other clause that precedes any search clause). They are ***unexpected*** anywhere else in the statement. This makes a Quelle statement a bit less cluttered than the version 1.0 specification

---

An object model that manifests a representation of Quelle grammar is depicted below:

![QCommand](./QCommand.png)

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

hyphen+colon ( **-:** ) means that any non-match satisfies the search condition. Used by itself, it would likely return every verse. Therfeore, it should be used judiciously.

### Appendix B. Specialized Search tokens in Quelle-AVX (a superset of Vanilla-Quelle Level 2)

Quelle-AVX, this includes all words in the original KJV text. It can optionally also search for modernized version of those words (e.g. hast and has; this is controllable with the %exact setting).  The table below lists additional linguistic extensions available in Quelle-AVX.

| Search Term  | Operator Type      | Meaning                                                      | Maps To                                | Mask     |
| ------------ | ------------------ | ------------------------------------------------------------ | -------------------------------------- | -------- |
| Jer\*        | wildcard           | starts with Jer                                              | selects from lexicon                   | 0x3FFF   |
| \*iah        | wildcard           | ends with iah                                                | selects from lexicon                   | 0x3FFF   |
| Jer\*iah     | wildcard           | starts with Jer and ends with iah                            | Jer\* & \*iah                          | as above |
| \\is\\       | lemma              | search on all words that share the same lemma as is: be, is, are, art, etc | be\|is\|are\|art\|...                  | 0x3FFF   |
| /noun/       | lexical marker     | any word where part of speech is a noun                      | POS12::0x010                           | 0x0FF0   |
| /n/          | lexical marker     | synonym for /noun/                                           | POS12::0x010                           | 0x0FF0   |
| /!n/         | lexical marker     | any word where part of speech is not a noun                  | POS12::0x010                           | 0x0FF0^  |
| /verb/       | lexical marker     | any word where part of speech is a verb                      | POS12::0x100                           | 0x0FF0   |
| /v/          | lexical marker     | synonym for /verb/                                           | POS12::0x100                           | 0x0FF0   |
| /!v/         | lexical marker     | any word where part of speech is not a verb                  | POS12::0x100                           | 0x0FF0^  |
| /pronoun/    | lexical marker     | any word where part of speech is a pronoun                   | POS12::0x020                           | 0x0FF0   |
| /pn/         | lexical marker     | synonym for /pronoun/                                        | POS12::0x020                           | 0x0FF0   |
| /!pn/        | lexical marker     | any word where part of speech is not a pronoun               | POS12::0x020                           | 0x0FF0^  |
| /adjective/  | lexical marker     | any word where part of speech is an adjective                | POS12::0xF00                           | 0x0FFF   |
| /adj/        | lexical marker     | synonym for /adjective/                                      | POS12::0xF00                           | 0x0FFF   |
| /!adj/       | lexical marker     | any word where part of speech is not an adjective            | POS12::0xF00                           | 0x0FFF^  |
| /adverb/     | lexical marker     | any word where part of speech is an adverb                   | POS12::0xA00                           | 0x0FFF   |
| /adv/        | lexical marker     | synonym for /adverb/                                         | POS12::0xA00                           | 0x0FFF   |
| /!adv/       | lexical marker     | any word where part of speech is not an adverb               | POS12::0xA00                           | 0x0FFF^  |
| /determiner/ | lexical marker     | any word where part of speech is a determiner                | POS12::0xD00                           | 0x0FF0   |
| /det/        | lexical marker     | synonym for /determiner/                                     | POS12::0xD00                           | 0x0FF0   |
| /!det/       | lexical marker     | any word where part of speech is not a determiner            | POS12::0xD00                           | 0x0FF0^  |
| /1p/         | lexical marker     | any word where it is inflected for 1st person (pronouns and verbs) | POS12::0x100                           | 0x3000   |
| /2p/         | lexical marker     | any word where it is inflected for 2nd person (pronouns and verbs) | POS12::0x200                           | 0x3000   |
| /3p/         | lexical marker     | any word where it is inflected for 3rd person (pronouns, verbs, and nouns) | POS12::0x300                           | 0x3000   |
| /singular/   | lexical marker     | any word that is known to be singular (pronouns, verbs, and nouns) | POS12::0x400                           | 0xC000   |
| /plural/     | lexical marker     | any word that is known to be plural (pronouns, verbs, and nouns) | POS12::0x800                           | 0xC000   |
| /WH/         | lexical marker     | any word that is a WH word (e.g., Who, What, When, Where, How) | POS12::0xC00                           | 0xC000   |
| /BoB/        | transition marker  | any word where it is the first word of the book (e.g. first word in Genesis) | TRAN::0xE0                             | 0xF0     |
| /BoC/        | transition marker  | any word where it is the first word of the chapter           | TRAN::0x60                             | 0xF0     |
| /BoV/        | transition marker  | any word where it is the first word of the verse             | TRAN::0x20                             | 0xF0     |
| /EoB/        | transition marker  | any word where it is the last word of the book (e.g. last word in revelation) | TRAN::0xF0                             | 0xF0     |
| /EoC/        | transition marker  | any word where it is the last word of the chapter            | TRAN::0x70                             | 0xF0     |
| /EoV/        | transition marker  | any word where it is the last word of the verse              | TRAN::0x30                             | 0xF0     |
| /!BoB/       | transition marker  | any word where it is not the first word of the book          | TRAN::0xE0                             | 0xF0^    |
| /!BoC/       | transition marker  | any word where it is not the first word of the chapter       | TRAN::0x60                             | 0xF0^    |
| /!BoV/       | transition marker  | any word where it is not the first word of the verse         | TRAN::0x20                             | 0xF0^    |
| /!EoB/       | transition marker  | any word where it is not the last word of the book           | TRAN::0xF0                             | 0xF0^    |
| /!EoC/       | transition marker  | any word where it is not the last word of the chapter        | TRAN::0x70                             | 0xF0^    |
| /!EoV/       | transition marker  | any word where it is not the last word of the verse          | TRAN::0x30                             | 0xF0^    |
| /Hsm/        | segment marker     | Hard Segment Marker (end) ... one of \. \? \!                | TRAN::0x40                             | 0x07     |
| /Csm/        | segment marker     | Core Segment Marker (end) ... \:                             | TRAN::0x20                             | 0x07     |
| /Rsm/        | segment marker     | Real Segment Marker (end) ... one of \. \? \! \:             | TRAN::0x60                             | 0x07     |
| /Ssm/        | segment marker     | Soft Segment Marker (end) ... one of \, \; \( \) --          | TRAN::0x10                             | 0x07     |
| /sm/         | segment marker     | Any Segment Marker (end)  ... any of the above               | TRAN::!=0x00                           | 0x07     |
| /_/          | punctuation        | any word that is immediately marked for clausal punctuation  | PUNC::!=0x00                           | 0xE0     |
| /!/          | punctuation        | any word that is immediately followed by an exclamation mark | PUNC::0x80                             | 0xE0     |
| /?/          | punctuation        | any word that is immediately followed by a question mark     | PUNC::0xC0                             | 0xE0     |
| /./          | punctuation        | any word that is immediately followed by a period (declarative) | PUNC::0xE0                             | 0xE0     |
| /-/          | punctuation        | any word that is immediately followed by a hyphen/dash       | PUNC::0xA0                             | 0xE0     |
| /;/          | punctuation        | any word that is immediately followed by a semicolon         | PUNC::0x20                             | 0xE0     |
| /,/          | punctuation        | any word that is immediately followed by a comma             | PUNC::0x40                             | 0xE0     |
| /:/          | punctuation        | any word that is immediately followed by a colon (information follows) | PUNC::0x60                             | 0xE0     |
| /'/          | punctuation        | any word that is possessive, marked with an apostrophe       | PUNC::0x10                             | 0x10     |
| /)/          | parenthetical text | any word that is immediately followed by a close parenthesis | PUNC::0x0C                             | 0x0C     |
| /(/          | parenthetical text | any word contained within parenthesis                        | PUNC::0x04                             | 0x04     |
| /Italics/    | text decoration    | italicized words marked with this bit in punctuation byte    | PUNC::0x02                             | 0x02     |
| /Jesus/      | text decoration    | words of Jesus marked with this bit in punctuation byte      | PUNC::0x01                             | 0x01     |
| /delta/      | lexicon            | [archaic] word can be transformed into modern American English |                                        |          |
| /!delta/     | lexicon            | a word is not archaic (spelling is consistent with modern American English) |                                        |          |
| \#FFFF       | PN+POS(12)         | hexadecimal representation of bits for a PN+POS(12) value.   | See Digital-AV SDK                     | uint16   |
| \#FFFFFFFF   | POS(32)            | hexadecimal representation of bits for a POS(32) value.      | See Digital-AV SDK                     | uint32   |
| #string      | nupos-string       | NUPOS string representing part-of-speech. This is the preferred syntax over POS(32), even though they are equivalent. NUPOS part-of-speech values have higher fidelity than the 16-bit PN+POS(12) representations. | See Part-of-Speech-for-Digital-AV.docx | uint64   |
| 99999:H      | Strongs Number     | decimal Strongs number for the Hebrew word in the Old Testament | One of Strongs\[4\]                    | 0x7FFF   |
| 99999:G      | Strongs Number     | decimal Strongs number for the Greek word in the New Testament | One of Strongs\[4\]                    | 0x7FFF   |

### Appendix C. Object Model to support search tokens in Quelle-AVX

An object model to support specialized Search Tokens for Quelle-AVX is depicted below:

![QFind](./QFind.png)



### Appendix D. Notional YAML for search interop (a search-oriented subset of Blueprint-Blue)

[EXAMPLE depicted as YAML; see Appendix F for FlatBuffer IDL definition]

```yaml
settings:
  exact: false
  span: 7,
  lexicon: av
  format: html
  
scope:
  - include: Hebrews

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



​		

### Appendix E. Notional YAML representation of search result (actual result is a flatbuffer)

[EXAMPLE depicted as YAML; see Appendix F for FlatBuffer IDL definition]

```yaml
settings:
  exact: false
  span: 7,
  lexicon: av
  format: html
  
scope: 
  - include: Hebrews

results:
  - find: time|help&/!verb/ ... need
    negate: false
    - found: 0x58041620
      length: 5
      - fragment: time|help
        feature: time 
        match: 0x58041620
      - fragment: /!verb/
        feature: /!verb/
        match: 0x58041621
      - fragment: need
        feature: need 
        match: 0x58041624
    - found: 0x58041622
      length: 3
      - fragment: time|help
        feature: help 
        match: 0x58041622
      - fragment: /!verb/
        feature: /!verb/
        match: 0x58041623
      - fragment: need
        feature: need 
        match: 0x58041624
```

### Appendix F. FlatBuffer schema for request/reply of searches to search-provider

```protobuf
attribute "fs_serializer";

namespace Blueprint.FlatBuf;

enum XLexEnum:  byte { AV = 0, AVX = 1 }
enum XFmtEnum:  byte { JSON = 0, TEXT = 1, HTML = 2, MD = 3 }
enum XLangEnum: byte { H = 1, G = 2, X = 0 }

table XRequest (fs_serializer) {
    settings:    XSettings   (required);
    scope:     [ XScope ];
    search:      XRequest    (required);
    render:      XScope;
}

table XSearch (fs_serializer) {
    search:      string      (required);
    negate:      bool        = false;
    quoted:      bool        = false;
    segments:  [ XSegment ]  (required);
}

table XSegment (fs_serializer) {
    anchored:    bool         = false;
    segments:  [ XFragment ]  (required);
}

table XFragment (fs_serializer) {
    features:  [ XFeature ]   (required);
}

table XFeature (fs_serializer) {
    text:        string      (required);
    rule:        string      (required);
    negate:      bool        = false;
    match:       XCompare    (required);
}

union XCompare (fs_serializer) {
    text:        XWord,
    lemma:       XLemma,
    pos16:       XPOS16,
    pos32:       XPOS32,
    punctuation: XPunctuation,
    strongs:     XStrongs,
    delta:       XDelta
}

table XWord (fs_serializer) {
    values:    [ uint16 ] (required);
}

table XLemma (fs_serializer) {
    values:    [ uint16 ] (required);
}

table XPOS32 (fs_serializer) {
    pos:         uint32;
}

table XPOS16 (fs_serializer) {
    pnpos:       uint16;
}

table XPunctuation (fs_serializer) {
    bits:        uint8;
}

table XStrongs (fs_serializer) {
    lang:        XLangEnum = X;    
    number:      uint16;
}

table XDelta (fs_serializer) {
    differs:     bool      = true; // must be explicitly set to T or F
}

table XSettings (fs_serializer) {
    exact:       bool     = false;
    span:        uint16   = 0;
    lexicon:     XLexEnum = AV;
    format:      XFmtEnum = JSON;
}

table XScope (fs_serializer) {
    book:        uint8 = 0;      // required
    chapter:     uint8 = 0;      // required
    verse:       uint8 = 1;      // optional
    vcount:      uint8 = 255;    // optional: verse-count: defaults to all remaining verses in chapter
}

table XResults (fs_serializer) {
    settings:    XSettings (required);
    scope:     [ XScope ]  (required);
    matches:   [ XFind ];
    errors:    [ string ];
    warnings:  [ string ];
}

table XFind (fs_serializer) {
    search:      string    (required);
    negate:      bool      = false;
    found:     [ XFound ];
}

table XFound (fs_serializer) {
    start:       uint32 = 0; // required;
    until:       uint32 = 0; // required;
    matches:   [ XMatch ]  (required);
}

table XMatch (fs_serializer) {
    fragment:    string    (required);
    feature:     string    (required);
    match:       uint32 = 0; // required; match represents coordinates of matching feature: BB_CC_VV_WW
}
```

### Appendix G. Extern C functions for Searching and Rendering

```C++
extern "C" __declspec(dllexport) const std::uint8_t* avx_create_search(const std::uint8_t* const request);
extern "C" __declspec(dllexport) void avx_delete_search(const uint8_t* const* results);

extern "C" __declspec(dllexport) const char* avx_create_rendering(const uint8_t* const spec);
extern "C" __declspec(dllexport) void avx_delete_rendering(const char* const rendering);
//
// Notionally (pseudo-code / FlatBuffers not withstanding):
//
class xresults;
class xrequest;
class xrender;
//
// classes forward referenced above are defined in blueprint.fbs [i.e. FratBuffers IDL is depicted in Appendix F]
//
extern "C" xresults* avx_create_search(xrequest* request);
extern "C" void avx_delete_search(xresults* results);

extern "C" char* avx_create_rendering(xrender* spec);
extern "C" void avx_delete_rendering(char* rendering);
```



​	

​	
