# Quelle Specification for AVX Framework

##### AVX-Quelle version 2.0.4.101

### I. Background

Most modern search engines, provide a mechanism for searching via a text input box, where the user is expected to type search terms. While this interface is primitive from a UI perspective, it facilitates rich augmentation via search-specific semantics. Google pioneered the modern search interface by employing an elegantly simple "search box". This was an evolution away from the complex interfaces that preceded it. However, when a user searches for multiple terms, it becomes confusing how to obtain any results except "match every term".

The vast world of search is rife for a standardized search-syntax that moves us past only basic search capabilities. Without the introduction of a complicated search UI, Quelle represents a freely available specification for an open Human-Machine-Interface (HMI). It can be easily invoked from within a simple text input box on a web page or even from a specialized command shell. The syntax supports Boolean operations such as AND, OR, and NOT, albeit in a non-gear-headed way. While great care has been taken to support the construction of complex queries, greater care has been taken to maintain a clear and concise syntax.

Quelle, IPA: [kɛl], in French means "What? or Which?". As Quelle HMI is designed to obtain search-results from search-engines, this interrogative nature befits its name. An earlier interpreter, Clarity, served as inspiration for defining Quelle.  You could think of the Quelle HMI as the next generation of Clarity HMI.  Yet, in order to produce linguistic consistency, Quelle syntax varied dramatically from the Clarity spec. Therefore, a new name was the best way forward.  Truly, Quelle HMI incorporates lessons learned after creating, implementing, and revising Clarity HMI for over a decade.

In 2023, Quelle 2.0 was released. This new release is not a radical divergence from version 1. Most of the updates to the specification are related to macros, control variables, filters, and export directives. Search syntax has remained largely unchanged. We discovered some ambiguity in the PEG grammar that implements the Quelle parser. Certain implicit actions could not be defined deterministically. To resolve those ambiguities, Quelle syntax was refined and updated. Consequently, new operators were introduced  [We added $ % and || to name a few] . These new operators eliminate the need for clause delimiters, in most circumstances. As a result, the version 2 syntax is more streamlined and more intuitive. It comes with a reference implementation in Rust and a fully specified PEG grammar.  Implicit actions for Macros are now referred to as *apply* and *invoke* [those verbs replace *save* and *execute* respectively].

Quelle is consistent with itself, to make it feel intuitive. Some constructs make parsing unambiguous; other constructs increase ease of typing (Specifically, we attempt to minimize the need to press the shift-key). Naturally, existing scripting languages also influence our syntax. Quelle represents an easy to type and easy to learn HMI.  Moreover, simple search statements look no different than they might appear today in a Google or Bing search box. Still, let's not get ahead of ourselves or even hint about where our simple specification might take us ;-)

Finally, Quelle 2.0 has dropped support for "bracketed search terms". Bracketed search terms allowed some terms within a quoted string to be unordered. Bracketing signaled that those terms could be in any order. Issues surfaced when defining the blueprint-blue object model. Some terms needed to be in a specific order and others did not, all within the same search construct. To make matters worse, the construct was seldom used. Notwithstanding, bracketed terms were even difficult to explain. Rather than tackle these problems for such a minor feature, we elected to nix bracketed terms altogether in Quelle.

### II. Addendum

Vanilla Quelle specifies two possible implementation levels:

- Level 1 [basic search support]
- Level 2 [search support includes also searching on part-of-speech tags]

AVX-Quelle is a Level 2 implementation with augmented search capabilities. AVX-Quelle extends baseline Vanilla-Quelle to include AVX-specific constructs.  These extensions provide additional specialized search features and the ability to manage two distinct lexicons for the biblical texts.

1. AVX-Quelle represents the biblical text with two substantially similar, but distinct, lexicons. The %lexicon setting can be specified by the user to control which lexicon is to be searched. Likewise, the %display setting is used to control which lexicon is used for displaying the biblical text. As an example, the KJV text of "thou art" would be modernized to "you are".

   - AV/KJV *(a lexicon that faithfully represents the KJV bible; AV purists should select this setting)*

   - AVX/Modern *(a lexicon that that has been modernized to appear more like contemporary English)*

   - Dual/Both *(use both lexicons for searching; this setting is not compatible with the %display setting)*

2. AVX-Quelle provides support for fuzzy-match-logic. The %similarity setting can be specified by the user to control the similarity threshold for approximate matching. An exact lexical match is expected when %similarity is set to *exact* or 0.  Zero is not really a similarity threshold, but rather 0 is a synonym for *exact*.

   Approximate matches are considered when similarity is set between 33 and 99 (33% to 99%). Similarity is calculated based upon the phonetic representation for the word.

   The minimum permitted similarity threshold is 33%. Any similarity threshold between 1 and 32 produces a syntax error.

   A %similarity setting of *precise* or 100 is a special case that still uses phonetics, but expects a full phonetic match (e.g. "there" and "their" are a 100% phonetic match).

3. In a future release, similarity matching on lemmas can also be specified. Similarity matching on lemmas will be enabled by appending an exclamation mark ( ! ) to the similarity threshold (e.g. %similarity = 75!). Automatic matching on lemmas is not supported when %similarity is set to *exact* or 0. The PEG grammar already supports the exclamation mark in the syntax. However, it is currently ignored in AVX-Search. A future version of AVX-Search will make this additional feature available.

AVX-Quelle uses the AV-1769 edition of the sacred text. It substantially agrees with the "Bearing Precious Seed" bibles, as published by local church ministries. The text itself has undergone review by Christian missionaries, pastors, and lay people since the mid-1990's. The original incarnation of the digitized AV-1769 text was implemented in the free PC/Windows app known as:

- AV-1995
- AV-1997
- AV-1999
- AV-2000
- AV-2007
- AV-2011
- AV-Bible for Windows

These releases were found at the [older/legacy] avbible.net website. Initially [decades ago], these releases were found on internet bulletin boards and the [now defunct] bible.advocate.com website.

Please see https://Digital-AV.org for additional information about the SDK.

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

Quelle is an open and extensible standard, additional verbs, and verbs for other languages can be defined without altering the overall syntax structure of the Quelle HMI. The remainder of this document describes Version 2.0 of the Quelle-HMI Level-II specification with specialized AVX search augmentations.  Moreover, there is no need to consult the Vanilla-Quelle documentation, as all similarities with Vanilla-Quelle are redundantly documented here.

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
4. OUTPUT
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

Each clause has either a single explicit action or any number of implicit actions.  Explicit actions begin with the @ symbol, immediately followed by the explicit verb.  Implicit actions are inferred by the syntax of the command.

### IV. Fundamental Quelle Commands

Learning just six verbs is all that is necessary to effectively use Quelle. In the table below, each verb is identified with required and optional parameters/operators.

| Verb      | Action Type | Syntax Category | Required Parameters       | Alternate/Optional Parameters |
| --------- | :---------: | :-------------- | ------------------------- | :---------------------------: |
| *find*    |  implicit   | SEARCH          | *search spec*             |                               |
| *filter*  |  implicit   | SEARCH          | **<** *domain*            |                               |
| *set*     |  implicit   | CONTROL         | **%name** **=** *value*   |   **%name** **:=** *value*    |
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
| SEARCH filters                          | < Genesis < Exodus < Revelation          |
| SEARCH specification                    | this is some text expected to be found   |
| Compound statement: two SEARCH actions  | "this quoted text" + other unquoted text |
| Compound statement: two CONTROL actions | %span = 7 %similarity = 85               |
| Compound statement: CONTROL & SEARCH    | %span = 7 Moses said                     |

**TABLE 4-2** -- **Examples of Quelle statement types**

Consider these two examples of Quelle statements (first CONTROL; then SEARCH):

%lexicon := KJV

"Moses"

Notice that both statements above are single actions.  We should have a way to express both of these in a single command. And this is the rationale behind a compound statement. To combine the previous two actions into one compound statement, issue this command:

"Moses" %lexicon:=KJV

### V. Deep Dive into Quelle SEARCH actions

Consider this proximity search where the search using Quelle syntax:

*span=7  Moses Aaron*

Quelle syntax can define the lexicon by also supplying an additional CONTROL action:

*%span=7 %lexicon=KJV  Moses Aaron*

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

Quelle is designed to be intuitive. It provides the ability to invoke Boolean logic for term-matching and/or linguistic feature-matching. As we saw above, the pipe symbol ( | ) can be used to invoke an *OR* condition In effect, this invokes Boolean multiplication on the terms and features that compose the expression.

The ampersand symbol can similarly be used to represent *AND* conditions upon terms. As an example. the English language contains words that can sometimes as a noun , and other times as a noun or other part-of-speech. To determine if the bible text contains the word "part" where it is used as a verb, we can issue this command:

"part&/verb/"

The SDK, provided by Digital-AV, has marked each word of the bible text for part-of-speech. With Quelle's rich syntax, this type of search is easy and intuitive.

Of course, part-of-speech expressions can also be used independently of an AND condition, as follows:

%span = 6 + "/noun/ ... home"

That search would find phrases where a noun appeared within a span of six words, preceding the word "home"

**Valid statement syntax, but no results:**

this&that

/noun/ & /verb/

Both of the statements above are valid, but will not match any results. Search statements attempt to match actual words in  the actual bible text. A word cannot be "this" **and** "that". Likewise, an individual word in a sentence does not operate as a /noun/ **and** a /verb/.

**Negating search-terms Example:**

Consider a query for all passages that contain a word beginning with "Lord", followed by any word that is neither a verb nor an adverb:

%span = 15 "Lord\* -/v/ & -/adv/"

this|that

/noun/ | /verb/

### VI. Displaying Results

Consider that there are two fundamental types of searches:

- Searches that return a limited set of results
- Searches that either return loads of results; or searches where the result count is unknown (and potentially very large)

Due to the latter condition above, SEARCH summarizes results (it does NOT display every result found). However, if more than a summary is desired, the user can control how many results to display.

"Jesus answered"			*summarize documents that contain this phrase, with paragraph references*

"Jesus answered" [ ]        *display every matching phrase*

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



| Verb     | Action Type | Syntax Category | Parameters       | Alternate #1      | Alternate #2      |
| -------- | :---------: | --------------- | ---------------- | :---------------- | :---------------- |
| *export* |  implicit   | OUTPUT          | **>** *filename* | **=>** *filename* | **>>** *filename* |

**TABLE 7-1** -- **The implicit export action**

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

### IX. Labeling & Reviewing Statements for subsequent invocation

| Verb            | Action Type | Syntax Category           | Expected Arguments                   | Required Operators |
| --------------- | ----------- | ------------------------- | ------------------------------------ | :----------------: |
| *invoke*        | implicit    | HISTORY & LABELING        | *label* or *id*                      |   **$** *label*    |
| *apply*         | implicit    | HISTORY & <u>LABELING</u> | *label*                              |  **\|\|** *label*  |
| **@delete**     | explicit    | HISTORY & <u>LABELING</u> | *label*                              |      *label*       |
| **@expand**     | explicit    | HISTORY & LABELING        | *label* or *id*                      |  *label* or *id*   |
| **@absorb**     | explicit    | CONTROL                   | *label* or *id*                      |  *label* or *id*   |
| **@review**     | explicit    | <u>HISTORY</u> & LABELING | **optional:** *max_count date_range* |                    |
| **@initialize** | explicit    | <u>HISTORY</u> & LABELING |                                      |                    |

**TABLE 9-1** -- **Labeling statements and reviewing statement history**

In this section, we will examine how user-defined macros are used in Quelle.  A macro in Quelle is a way for the user to label a statement for subsequent use.  By applying a label to a statement, a shorthand mechanism is created for subsequent invocation. This gives rise to two new definitions:

1. Labeling a statement (or defining a macro; labels must begin with a letter; never a number, underscore, or hyphen)

2. Invoking a labeled statement (running a macro)


Let’s say we want to name the search example from the previous section; We’ll call it *eternal-power*. To accomplish this, we would issue this command:

%span=7 %similarity=85 + eternal power || eternal-power

It’s that simple, now instead of typing the entire statement, we can utilize the macro by referencing our previously applied label. Here is how the macro can be invoked. We might call this running the macro:

$eternal-power

Labeled statements also support compounding using the semi-colon ( ; ), as follows; we will label it also:

$eternal-power + godhead|| my-label-cannot-contain-spaces

Later I can issue this command:

$my-label-cannot-contain-spaces

Which is equivalent to these statements:

%span=7  %similarity=85 + eternal power + godhead

There are several restrictions on macro definitions:

1. Macro definition must represent a valid Quelle statement:
   - The syntax is verified prior to applying the statement label.
2. Macro definitions exclude export directives
   - Any portion of the statement that contains > is incompatible with a macro definition
3. Macro definitions exclude output directives
   - Any portion of the statement that contains [ ] is incompatible with a macro definition
4. The statement cannot represent an explicit action:
   - Only implicit actions are permitted in a labeled statement.

Finally, any macros referenced within a macro definition are expanded prior to applying the new label. Therefore redefining a macro after it has been referenced in a subsequent macro definition has no effect of the initial macro reference. We call this macro-determinism.  A component of determinism for macros is that the macro definition saves all control settings at the time that the label was applied. This assure that the same search results are returned each time the macro is referenced. Here is an example.

%span := 2

in beginning || in_beginning

%span := 3

$in_beginning [1] < genesis:1:1

***result:*** none

However, if the user desires the current settings to be used instead, just include ***::current*** suffix after the macro. 

$in_beginning::current [1] < genesis:1:1

***result:*** Gen 1:1 In the beginning, God created ...

It should be noted that when a macro is paired with any other search clauses, it implicitly adds the settings::current suffix to all macros. Incidentally, the @expand command for history and macros reveals which settings are bundled in the history or macro.

##### Executing a macro remembers all settings, but always without side-effects:

A macro definition captures all settings. We have already discussed macro-determinism (saving settings utilized for execution is needed to provide macro determinism). However, when a macro is saved, any setting with an persistent assignment (e.g. span:=7) is treated as if it were a volatile assignment (span=7). In other words, executing a macro never persists changes into your environment, unless you explicitly request such behavior with the @absorb command.

##### Additional explicit macro commands:

Two additional explicit commands exist whereby a macro can be manipulated. We saw above how they can be defined and referenced. There are two additional ways commands that operate on macros: expansion and deletion.  In the last macro definition above where we created  $another-macro, the user could preview an expansion by issuing this command:

@expand another-macro

If the user wanted to remove this definition, the @delete action is used.  Here is an example:

@delete another-macro

If you want the same settings to be persisted to your current session that were in place during macro definition, the @absorb command will persist all settings for the macro into your current session

@absorb my-favorite-settings-macro 

Both @absorb and @expand also work with command history.

**NOTE:** Labels must begin with a letter [A-Z] or [a-z], but they may contain numbers, hyphens, periods, commas, underscores, and single-quotes (no other punctuation or special characters are supported).

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

**REVIEW SEARCH HISTORY**

*@review* gets the user's search activity for the current session.  To show the last ten searches, type:

*@review*

To show the last three searches, type:

*@review* 3

To show the last twenty searches, type:

*@review* 20 

To show the last twenty searches using date ranges, type any of:

*@review* after since 2023/07/04

*@review* until 2023/07/04

*@review* after 2023/07/04 until 2024/07/04 

*@review* 20 after 2023/07/04 until 2024/07/04 

All ranges are inclusive. Therefore, commands on July 4th would also be included.

**INVOKE**

The *invoke* command works for command-history works exactly the same way as it does for macros.  After issuing a *@review* command, the user might receive a response as follows.

*@review*

1>  %span := 7

2>  %similarity=85

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

%span := 7  %similarity=85   eternal power

**RESETTING COMMAND HISTORY**

The @reset command can be used to clear command history and/or reset all control variables to defaults.

To clear all command history:

@initialize history

##### Invoking a command remembers all settings, but always without side-effects:

Command history captures all settings. We have already discussed macro-determinism. Invoking commands by their review numbers behave exactly like macros. They are also deterministic. Just like macros, a setting with a persistent assignment (e.g. span:=7) is treated as if it were volatile (span=7). In other words, invoking command history never persists changes into your environment, unless you explicitly request such behavior with the @absorb command.

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

| Verb     | Action Type | Syntax Category | Parameters            | Alternate #1           | Alternate #2 |
| -------- | :---------: | --------------- | --------------------- | :--------------------- | :----------- |
| *clear*  |  implicit   | CONTROL         | *%name* **= default** | *%name* **:= default** |              |
| **@get** |  explicit   | CONTROL         | **optional:** *name*  |                        |              |

**TABLE 12-1** -- **Listing of additional CONTROL actions**



**Export Format Options:**

| **Markdown**   | **HTML**         | **Text**         | Json             |
| -------------- | ---------------- | ---------------- | ---------------- |
| *%format = md* | *%format = html* | *%format = text* | *%format = json* |

**TABLE 12-2** -- **set** format command can be used to set the default content-formatting for for use with the export verb



| **example**      | **explanation**                                              |
| ---------------- | ------------------------------------------------------------ |
| %span := 8       | Assign a control setting                                     |
| **@get** span    | get a control setting                                        |
| %span := default | Clear the control setting; restoring the Quelle driver default setting |

**TABLE 12-3** -- **set/clear/get** action operate on configuration settings



**SETTINGS:**

Otherwise, when multiple clauses contain the same setting, only the last setting in the list is preserved.  Example:

format=md   format=default  format=text

@get format

The final command would return text.  We call this: "last assignment wins". However, there is one caveat to this precedence order: regardless of where in the statement a macro or history invocation is provided within a statement, it never has precedence over a setting that is actually visible within the statement.  Consider this sequence as an example:

%similarity=none || precedence_example

%similarity=85  $precedence_example

@get %similarity

The final command would return 85, because it was visible in the compound statement.

AVX-Quelle manifests five control names. Each allows all three actions: ***set***, ***clear***, and ***@get*** verbs. Table 12-4 lists all settings available in AVX-Quelle. AVX-Quelle can support two distinct orthographies [i.e. Contemporary Modern English (avx/modern), and/or Early Modern English (avx/kjv).

| Setting    | Meaning                                                      | Values                                                       | Default Value |
| ---------- | ------------------------------------------------------------ | ------------------------------------------------------------ | ------------- |
| span       | proximity distance limit                                     | 0 to 999 or verse                                            | 0 [verse]     |
| lexicon    | the lexicon to be used for the searching                     | av/avx/dual (kjv/modern/both)                                | dual (both)   |
| display    | the lexicon to be used for display/rendering                 | av/avx (kjv/modern)                                          | av (kjv)      |
| format     | format of results on output                                  | see Table 12-2                                               | json          |
| similarity | fuzzy phonetics matching threshold is between 1 and 99<br/>0 or *none* means: do not match on phonetics (use text only)<br/>100 or *exact* means that an *exact* phonetics match is expected | 33 to 99 [fuzzy] **or** ...<br>0 **or** *none*<br>100 **or** *exact*<br>Exclamation ( ! ) after the value enables lemma matching (see Section II / item #3) | 0 (none)      |

**TABLE 12-4** -- **Summary of AVX-Quelle Control Names**

The *@get* command fetches these values. The *@get* command requires a single argument. Examples are below:

*@get* search

@get format

There are additional actions that affect all control settings collectively

| Expressions | Meaning / Usage                                              |
| ----------- | ------------------------------------------------------------ |
| **@reset**  | Reset is an explicit command alias to *clear* all control settings, resetting them all to default values<br />equivalent to: %span:=default %lexicon:=default %display:=default %similarity:=default %format:=default |
| $X::current | Special suffix for use with History or Macro invocation as a singleton statement:<br />See "Labeling Statements for subsequent invocation" section of this document.<br />Uses current settings for invocation on history/macro identified/labeled as "X".<br>(On non-singleton invocations, environment settings on the macro/history is **always** ignored, making the ::current suffix superfluous on compound macro satements) |

**TABLE 12-5** -- **Collective CONTROL operations**

All settings can be cleared using an explicit command:

@reset

It is exactly equivalent to this compound statement:

%span:=default  %lexicon:=default  %display:=default  %similarity:=default %format:=default

**Scope of Settings [Statement Scope vs Persistent Scope]**

It should be noted that there is a distinction between name:=value and name=value syntax variations. The first form is persistent with respect to subsequent statements. Contrariwise, the latter form affects only the single statement wherewith it is executed. We refer to this as variable scope, Consider these two very similar command sequences:

| Example of Statement Scope | Example of Persistent Scope |
| -------------------------- | --------------------------- |
| @reset controls            | @reset controls             |
| "Moses said" %span=7       | "Moses said" %span := 7     |
| "Aaron said"               | "Aaron said"                |

In the [volatile] **statement scope** example, setting span to "7" only affects the search for "Moses said". The next search for "Aaron said" utilizes the default value for span, which is "verse".

In the **persistent scope** example, setting span to "7" affects the search for "Moses said" <u>and all subsequent searches</u>. The next search for "Aaron said" continues to use a span value of "7'".   In other words, the setting **persists** <u>beyond the scope of statement execution</u>.

### XIII. Miscellaneous Information

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

Finally delimiters ( e.g.  ; or + ), between two distinct unquoted search clauses are ***required*** to identify the boundary between the two search clauses. A single delimiter <u>before</u> any search clause is always ***optional*** (e.g., Delimiters are permitted between quoted and unquoted clauses, and any other clause that precedes any search clause). They are ***unexpected*** anywhere else in the statement. This makes a Quelle statement a bit less cluttered than the version 1.0 specification

---

An object model to manifest the Quelle grammar is depicted below:

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

**not:** In Boolean logic, means that the feature must not be found. With Quelle, *not* is represented by the hyphen ( **-** ) and applies to individual features within a fragment of a search expression. It is best used in conjunction with other features, because any non-match will be included in results. 

hyphen ( **-** ) means that any non-match satisfies the search condition. Used by itself, it would likely return every verse. Therefore, it should be used judiciously.

### Appendix B. Specialized Search tokens in AVX-Quelle

The lexical search domain of AVX-Quelle includes all words in the original KJV text. It can also optionally search using a modernized lexicon of the KJV (e.g. hast and has; this is controllable with the %lexicon setting).  The table below lists additional linguistic extensions available in AVX-Quelle, which happens to be a Level-II Quelle implementation.

| Search Term   | Operator Type      | Meaning                                                      | Maps To                                | Mask   |
| ------------- | ------------------ | ------------------------------------------------------------ | -------------------------------------- | ------ |
| Jer\*         | wildcard           | starts with Jer                                              | selects from lexicon                   | 0x3FFF |
| \*iah         | wildcard           | ends with iah                                                | selects from lexicon                   | 0x3FFF |
| Jer\*iah      | wildcard           | starts with Jer and ends with iah                            | Jer\* & \*iah                          | 0x3FFF |
| \\is\\        | lemma              | search on all words that share the same lemma as is: be, is, are, art, ... | be\|is\|are\|art\|...                  | 0x3FFF |
| /noun/        | lexical marker     | any word where part of speech is a noun                      | POS12::0x010                           | 0x0FF0 |
| /n/           | lexical marker     | synonym for /noun/                                           | POS12::0x010                           | 0x0FF0 |
| /verb/        | lexical marker     | any word where part of speech is a verb                      | POS12::0x100                           | 0x0FF0 |
| /v/           | lexical marker     | synonym for /verb/                                           | POS12::0x100                           | 0x0FF0 |
| /pronoun/     | lexical marker     | any word where part of speech is a pronoun                   | POS12::0x020                           | 0x0FF0 |
| /pn/          | lexical marker     | synonym for /pronoun/                                        | POS12::0x020                           | 0x0FF0 |
| /adjective/   | lexical marker     | any word where part of speech is an adjective                | POS12::0xF00                           | 0x0FFF |
| /adj/         | lexical marker     | synonym for /adjective/                                      | POS12::0xF00                           | 0x0FFF |
| /adverb/      | lexical marker     | any word where part of speech is an adverb                   | POS12::0xA00                           | 0x0FFF |
| /adv/         | lexical marker     | synonym for /adverb/                                         | POS12::0xA00                           | 0x0FFF |
| /determiner/  | lexical marker     | any word where part of speech is a determiner                | POS12::0xD00                           | 0x0FF0 |
| /det/         | lexical marker     | synonym for /determiner/                                     | POS12::0xD00                           | 0x0FF0 |
| /preposition/ | lexical marker     | any word where part of speech is a preposition               | POS12::0x400                           | 0x0FF0 |
| /prep/        | lexical marker     | any word where part of speech is a preposition               | POS12::0x400                           | 0x0FF0 |
| /1p/          | lexical marker     | any word where it is inflected for 1st person (pronouns and verbs) | POS12::0x100                           | 0x3000 |
| /2p/          | lexical marker     | any word where it is inflected for 2nd person (pronouns and verbs) | POS12::0x200                           | 0x3000 |
| /3p/          | lexical marker     | any word where it is inflected for 3rd person (pronouns, verbs, and nouns) | POS12::0x300                           | 0x3000 |
| /singular/    | lexical marker     | any word that is known to be singular (pronouns, verbs, and nouns) | POS12::0x400                           | 0xC000 |
| /plural/      | lexical marker     | any word that is known to be plural (pronouns, verbs, and nouns) | POS12::0x800                           | 0xC000 |
| /WH/          | lexical marker     | any word that is a WH word (e.g., Who, What, When, Where, How) | POS12::0xC00                           | 0xC000 |
| /BoB/         | transition marker  | any word where it is the first word of the book (e.g. first word in Genesis) | TRAN::0xE0                             | 0xF0   |
| /BoC/         | transition marker  | any word where it is the first word of the chapter           | TRAN::0x60                             | 0xF0   |
| /BoV/         | transition marker  | any word where it is the first word of the verse             | TRAN::0x20                             | 0xF0   |
| /EoB/         | transition marker  | any word where it is the last word of the book (e.g. last word in revelation) | TRAN::0xF0                             | 0xF0   |
| /EoC/         | transition marker  | any word where it is the last word of the chapter            | TRAN::0x70                             | 0xF0   |
| /EoV/         | transition marker  | any word where it is the last word of the verse              | TRAN::0x30                             | 0xF0   |
| /Hsm/         | segment marker     | Hard Segment Marker (end) ... one of \. \? \!                | TRAN::0x40                             | 0x07   |
| /Csm/         | segment marker     | Core Segment Marker (end) ... \:                             | TRAN::0x20                             | 0x07   |
| /Rsm/         | segment marker     | Real Segment Marker (end) ... one of \. \? \! \:             | TRAN::0x60                             | 0x07   |
| /Ssm/         | segment marker     | Soft Segment Marker (end) ... one of \, \; \( \) --          | TRAN::0x10                             | 0x07   |
| /sm/          | segment marker     | Any Segment Marker (end)  ... any of the above               | TRAN::!=0x00                           | 0x07   |
| /_/           | punctuation        | any word that is immediately marked for clausal punctuation  | PUNC::!=0x00                           | 0xE0   |
| /!/           | punctuation        | any word that is immediately followed by an exclamation mark | PUNC::0x80                             | 0xE0   |
| /?/           | punctuation        | any word that is immediately followed by a question mark     | PUNC::0xC0                             | 0xE0   |
| /./           | punctuation        | any word that is immediately followed by a period (declarative) | PUNC::0xE0                             | 0xE0   |
| /-/           | punctuation        | any word that is immediately followed by a hyphen/dash       | PUNC::0xA0                             | 0xE0   |
| /;/           | punctuation        | any word that is immediately followed by a semicolon         | PUNC::0x20                             | 0xE0   |
| /,/           | punctuation        | any word that is immediately followed by a comma             | PUNC::0x40                             | 0xE0   |
| /:/           | punctuation        | any word that is immediately followed by a colon (information follows) | PUNC::0x60                             | 0xE0   |
| /'/           | punctuation        | any word that is possessive, marked with an apostrophe       | PUNC::0x10                             | 0x10   |
| /)/           | parenthetical text | any word that is immediately followed by a close parenthesis | PUNC::0x0C                             | 0x0C   |
| /(/           | parenthetical text | any word contained within parenthesis                        | PUNC::0x04                             | 0x04   |
| /Italics/     | text decoration    | italicized words marked with this bit in punctuation byte    | PUNC::0x02                             | 0x02   |
| /Jesus/       | text decoration    | words of Jesus marked with this bit in punctuation byte      | PUNC::0x01                             | 0x01   |
| /delta/       | lexicon            | [archaic] word can be transformed into modern American English |                                        |        |
| \#FFFF        | PN+POS(12)         | hexadecimal representation of bits for a PN+POS(12) value.   | See Digital-AV SDK                     | uint16 |
| \#FFFFFFFF    | POS(32)            | hexadecimal representation of bits for a POS(32) value.      | See Digital-AV SDK                     | uint32 |
| #string       | nupos-string       | NUPOS string representing part-of-speech. This is the preferred syntax over POS(32), even though they are equivalent. NUPOS part-of-speech values have higher fidelity than the 16-bit PN+POS(12) representations. | See Part-of-Speech-for-Digital-AV.docx | uint64 |
| 99999:H       | Strongs Number     | decimal Strongs number for the Hebrew word in the Old Testament | One of Strongs\[4\]                    | 0x7FFF |
| 99999:G       | Strongs Number     | decimal Strongs number for the Greek word in the New Testament | One of Strongs\[4\]                    | 0x7FFF |

### Appendix C. Object Model to support search tokens in Quelle-AVX

An object model to support specialized Search Tokens for Quelle-AVX is depicted below:

![QFind](./QFind.png)



### Appendix D. YAML representative example (subset of Blueprint-Blue for search)

```yaml
settings:
  similarity: 0 // none
  span: 0,      // verse
  lexicon: 1    // av
  format: html
  
scope: 
  - include: 58 // Hebrews

search:
  - find: time|help&-/verb/ ... need
    negate: false
    quoted: true
    - fragment: time|help&-/verb/
      anchored: true
      - option: time|help
        - feature: time 
          wkeys: [ 1316 ]
        - feature: help
          wkeys: [ 795 ]
      - option: -/verb/
        - feature: -/verb/
          negate: true
          pos16: 0x100
    - fragment: need
      anchored: false,
      - option: need
        - feature: need
          wkeys: [ 1026 ]
```





### Appendix E. YAML representation of results (TO DO: update to latest AVX spec)

```yaml
results:
  - find: time|help&-/verb/ ... need
    negate: false
    - found: 0x58041620
      length: 5
      - fragment: time|help
        feature: time 
        match: 0x58041620
      - fragment: -/verb/
        feature: -/verb/
        match: 0x58041621
      - fragment: need
        feature: need 
        match: 0x58041624
    - found: 0x58041622
      length: 3
      - fragment: time|help
        feature: help 
        match: 0x58041622
      - fragment: -/verb/
        feature: -/verb/
        match: 0x58041623
      - fragment: need
        feature: need 
        match: 0x58041624
```

### Appendix F. Notional schema of full blueprint-blue object model for AVX-Quelle

```yaml
enum XThreshold: byte { NONE = 0, FUZZY_MIN = 33, FUZZY_MAX = 99, EXACT = 100 }
enum XOutEnum:   byte { AV = 1, AVX = 2 }
enum XLexEnum:   byte { AV = 1, AVX = 2, BOTH = 3 }
enum XFmtEnum:   byte { JSON = 0, TEXT = 1, HTML = 2, MD = 3 }
enum XLangEnum:  byte { H = 1, G = 2, X = 0 }
enum XStatusEnum:byte { OKAY = 0, ERROR = -128 }

table XBlueprint { // was: XRequest
    command:     string      (required);
    settings:    XSettings   (required);
    search:    [ XSearch ];
    scope:     [ XScope ];
    singleton:   XCommand;
    status:      XStatusEnum = ERROR;  // required
    help:        string      (required); // url of help-file for singleton.command xor search.expression  [likely on github for automatic markdown to html conversion]
    warnings:  [ string ];
    errors:    [ string ];
}

table XCommand { // for singleton expressions
    command:     string      (required);
    verb:        string      (required);
    arguments: [ string ];
    reply:       XReply;
}

table XReply {
    version:     string;      // for @version
    help:        string;      // for @help
    value:       string;      // for @get
    macro:       XStatement;  // for @expand
    history:   [ XStatement ];// for @review (history)
}

table XStatement {
    id:          uint32 = 0;  // required for @review (history)
    label:       string;      // required for @expand reply
    time:        uint64 = 0;  // required
    stmt:        string      (required);
    expd:        string      (required);
    summary:     string;      // defined only for searches
    settings:    XSettings   (required);
}

table XSearch {
    expression:  string       (required); // segment
    quoted:      bool        = false;
    fragments:  [ XFragment ] (required);
}

table XFragment {
    fragment:    string      (required);
    anchored:    bool        = false;
    required:  [ XOption ]   (required); // AND conditions (all must match)
}

table XOption {
    option:      string      (required);
    features:  [ XFeature ]  (required); // OR conditions (any can match)
}

union XCompare {
    text:        XWord,
    lemma:       XLemma,
    pos16:       XPOS16,
    pos32:       XPOS32,
    punctuation: XPunctuation,
    transition:  XTransition,
    strongs:     XStrongs,
    delta:       XDelta
}

table XFeature (fs_serializer) {
    feature:     string      (required);
    rule:        string      (required);
    negate:      bool        = false;
    match:       XCompare    (required);
}

// Most of the Blueprint-Blue object model is directlty inferred from the Pinshot-Blue parse (Rust FFI).
// However, XWord is now generated using the NUPhone assembly (C#).
// This way, AVX-Search can perform fuzzy matches, w/o requiring a C++ implementation for English-to-IPA

table XLex {
    key:         uint16 = 0;             // zero is a valid value for OOV items (items found neither in lexicon, nor in OOV-Lemmata)
    phonetics: [ string ] (required);    // required, but may be empty (in the case of QWildcard: we do not find phonetic variants for all lexicon matches)
}

table XWord {
    lex:       [ XLex ] (required);
}

table XLemma {
    lemmata:   [ XLex ] (required);
}

table XPOS32 {
    pos:         uint32;
}

table XPOS16 {
    pnpos:       uint16;
}

table XPunctuation {
    bits:        uint8;
}

table XTransition {
    bits:        uint8;
}

table XStrongs {
    lang:        XLangEnum = X;    
    number:      uint16;
}

table XDelta {
    differs:     bool      = true; // must be explicitly set to T or F
}

table XSettings {
    similarity:  string   (required);
    span:        uint16   = 0;
    lexicon:     XLexEnum = BOTH;
    display:     XOutEnum = AV;
    format:      XFmtEnum = JSON;
}

table XScope {
    book:        uint8 = 0;      // required
    chapter:     uint8 = 0;      // required
    verse:       uint8 = 1;      // optional
    vcount:      uint8 = 255;    // optional: verse-count: defaults to all remaining verses in chapter
}
```

### Appendix G. Developer Notes

- Quelle expressions have one or more Quelle commands or segments
- Search segments utilize the QFind class
- Search segments have one or more fragments separated by whitespace (all fragments are implicit AND conditions)
- Fragments have one or more MatchAny objects [MatchAny objects are explicit AND conditions, separated by & ]
- MatchAny objects have one or more Features [Features are explicit OR conditions, separated by | ]
- Features can be (negated) using the unary negation operator [ - ]

#### Example

**%span = 15 Lord\* -/v/ & -/adv/ + /v/|/n/&-run**

Two search segments (search segments are encapsulated by the QFind class):

​	search-segment-1: **Lord\* -/v/ & -/adv/**       (this has two fragments)

​		fragment-1 has a single MatchAny object

​			MatchAny object has one [wildcard] feature: **Lord***

​		fragment-2 has two oMatchAny objects (these are AND conditions)

​			MatchAny object-1 has a single negated feature: **-/v/** (NOT a VERB)

​			MatchAny object-2 has a single negated feature: **-/adv/** (NOT an ADVERB)

​	search-segment-2: **/v/|/n/ & -run** has a single fragment

​		The fragment has  two ANDed MatchAny objects

​			MatchAny object-1 has two ORed features: (IS a VERB) OR (IS a NOUN)

​			MatchAny object-2 has a single negated feature: (NOT the word RUN) 

NOTE: the plus sign between the two search segments above implies that results of the segments are ORed together.

​	
