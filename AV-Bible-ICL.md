<script src="./md-page.js"></script><noscript>

# Imperative Control Language (ICL) for AV-Bible

##### version 4.3.9

### Background

Near the end of the last century, Google pioneered the modern search interface by employing an elegantly simple "search box". This was an evolution away from the complex interfaces that preceded it. Still, it becomes problematic when we want to search for multiple terms, unless we expect to merely "match every term".

Imperative Control Language (ICL) provides a concise yet comprehensive command-language for searching, configuring, and controlling the AV-Bible application.  Its grammar supports Boolean operations such as AND, OR, and NOT. While great care has been taken to support the construction of complex queries, greater care has been taken to maintain a clear and concise syntax.

ICL is consistent with itself. Naturally, existing scripting languages have some influence on ICL syntax. However, we always favor simplicity over versatility. This avoids complexity of syntax and makes behavior easy to describe. There might be edge cases that where a more versatile grammar might have reduced keystrokes. Yet, by avoiding nuance, ICL is easy to learn, easy to type, and easy to remember.  In fact, search expressions often look no different than they would appear today in a Google or Bing search. Incidentally, ICL is a dialect of Quelle and conforms to the Quelle Specification.

### Overview of ICL Grammar

There are two types of statements

| Statement Type      | Syntax                                                       |
| ------------------- | ------------------------------------------------------------ |
| Selection  Criteria | Combines search criteria and scoping filters for tailored verse selection.<br/>Configuration settings can also be combined and incorporated into the selection criteria. |
| Discrete Imperative | single action for configuration and/or application control (cannot be combined with other actions) |

#### Selection Criteria (includes search operations)

Selection Criteria contains <u>required</u> Selection Criteria, followed by an <u>optional</u> Directive:

The selection criteria controls how verses are selected. It is made up of one to three blocks. The ordering of blocks is partly prescribed. The scoping block must be in the final position. The search-expression-block and settings-block can be in either order (so long as they are listed before the scoping block when present). 

- Search Expression Block
- Settings Block
- Scoping Block

|                         | Search Expression Block | Settings Block | Scoping Block |
| ----------------------- | ----------------------- | -------------- | ------------- |
| Block Position          | 0 or 1                  | 0 or 1         | final         |
| Macro utilization level | full                    | partial        | partial       |

An optional directive can be issued following the selection criteria.  Only zero or one directives can be issued within a  statement:  

- Macro Directive

- Export Directive

To be clear, a macro cannot be created for a statement that exports selection/search results to a file. Directives must be instigated separately.  The syntax for these directives is straightforward

| Directive Type                  | Directive Syntax *(follows the Selection Criteria)*          |
| ------------------------------- | ------------------------------------------------------------ |
| Macro (*apply* tag)             | ***\|\| tag***                                               |
| Export Block (*export* to file) | ***> filepath*** or<br/>***>> filepath*** or<br/>***=> filepath*** |

#### Discrete Imperatives

Non-selection statements instigate configuration changes or application control. These statements always begin with **@**. They are executed individually and cannot be combined with any other actions. Imperatives that begin with @ cannot be combined with search expressions. This is why they are called Discrete Imperatives.

#### Configuration Statements

ICL supports three categories of configuration. These are described more completely in Section 2.

| Configuration Targets | Configuration Actions       |
| --------------------- | --------------------------- |
| User Settings         | @set, @get, @clear, @absorb |
| User Macros           | @view, @delete              |
| User History          | @view, @delete, @invoke     |

#### Control Statements

ICL has only two control imperatives. These are described more completely in Section 3.

| Control Targets   | Control Actions | Optional Parameter | Description                         |
| ----------------- | --------------- | ------------------ | ----------------------------------- |
| Usage Information | @help           | topic              | Help with ICL syntax and usage      |
| System Control    | @exit           | -                  | Exit the application or interpreter |

## Section 1 - Selection/Search 

From a linguistic standpoint, all ICL statements are issued in the imperative. The subject of the verb is always "you understood". As the user, you are commanding the application what to do. Some statements have additional guiding parameters. These parameters instruct what the command is to act upon.

Consider these two examples of ICL statements (first Configuration, followed by Search):

@lexicon.searh = KJV

"Moses"

Notice that both statements above are single actions.  We should have a way to express both of these in a single command. And this is the rationale behind statements blocks and directives. Only search syntax supports combining various actions into a single statement. The previous two actions into a single compound statement, issue this command:

"Moses" [ search.lexicon:KJV ]

It should be noted that these two statements, while similar, do not have identical effects:

- @search.lexicon = KJV
- [ search.lexicon:KJV ]

The former, which is a configuration imperative, changes the lexicon setting for all future searches. Whereas, the latter, merely a Settings-Block assigment, affects only the currently executing Selection/Search imperative. Subsequent searches are unaffected by settings-block assignments. There are times when a user will want a setting to persist, and other times when the user wants the setting changed only temporarily. ICL permits the user to choose.

#### QuickStart

Consider this proximity search (find Moses and Aaron within a single span of seven words):

*[ span:7 ]  Moses Aaron*

ICL syntax can specify the lexicon to search, by also supplying temporary settings:

*[ span:7 search.lexicon:KJV ]  Moses Aaron*

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

ICL is designed to be intuitive. It provides the ability to invoke Boolean logic for term-matching and/or linguistic feature-matching. As we saw above, the pipe symbol ( | ) can be used to invoke an *OR* condition.

### 1.1 - Selection Criteria

As we saw in the overview, there three blocks that compose Selection Criteria:

- Expression Block Components

  - *find expression*
  - *use expression (full macro utilization)*
- Settings Block Components

  - *assign setting*

  - *use settings (partial macro utilization)*
- Scoping Block

  - *filter directives*
  - *use filters (partial macro utilization)*

| Action   | Type             | Position | Action Syntax                            | Repeatable Action                                        |
| -------- | ---------------- | -------- | ---------------------------------------- | -------------------------------------------------------- |
| *find*   | Expression Block | initial  | search expression or ***#id***           | **no**                                                   |
| *use*    | Expression Block | initial  | ***#tag*** or ***#id ***                 | **no**: only one macro is permitted per block            |
| *assign* | Settings Block   | initial  | ***[ setting: value ]***                 | yes (e.g. ***[ format:md  lexicon:kjv  span:verse ]*** ) |
| *use*    | Settings Block   | initial  | ***[ #tag ]*** or<br/>***[ #id ]***      | **no**: only one macro is permitted per block            |
| *filter* | Scoping Block    | post     | ***< scope***                            | yes (e.g. ***< Genesis 3 < Revelation 1-3***)            |
| *use*    | Scoping Block    | post     | **<** ***#tag***  or<br/>**<** ***#id*** | **no**: only one macro is permitted per block            |

**Table 1-1** - Summary of actions expressible in the Selection Criteria segment of a Selection/Search imperative statement

Two mutually exclusive optional directives can be issued following the selection criteria. 

#### 1.1.1 - Search Expression Block

The ampersand symbol can similarly be used to represent *AND* conditions upon terms. As an example. the English language contains words that can sometimes as a noun , and other times as some other part-of-speech. To determine if the bible text contains the word "part" where it is used as a verb, we can issue this command:

"part&/verb/"

The SDK, provided by Digital-AV, has marked each word of the bible text for part-of-speech. With the rich syntax of ICL, this type of search is easy and intuitive.

Of course, part-of-speech expressions can also be used independently of an AND condition, as follows:

[ span: 6 ]  "/noun/ ... home"

That search would find phrases where a noun appeared within a span of six words, preceding the word "home"

**Valid statement syntax, but no results:**

this&that

/noun/ & /verb/

Both of the statements above are valid, but will not match any results. Search statements attempt to match actual words in  the actual bible text. A word cannot be "this" **and** "that". Likewise, an individual word in a sentence does not operate as a /noun/ **and** a /verb/ at the same time.

**Negating search-terms Example:**

Consider a query for all passages that contain a word beginning with "Lord", followed by any word that is neither a verb nor an adverb:

[ span:15 ] "Lord\* -/v/ & -/adv/"

#### 1.1.2 - Settings Block

When the same setting appears more than once, only the last setting in the list is preserved.  Example:

[ format:md  format:text ]

@get format

The @get format command would return text.  We call this: "last assignment wins".

Finally, there is a bit more to say about the similarity setting, because it actually has three components. If we issue this command, it affects similarity in two distinct ways:

[  similarity: 85% ]

That command is a concise way of setting two values. It is equivalent to this command

[ similarity.word:85%  similarity.lemma:85% ]

That is to say, similarity is operative for the lexical word and also the lemma of the word. While not discussed previously, these two similarities thresholds need not be identical. These commands are also valid:

[ similarity.word: 85%  similarity.lemma: 95% ]

[ similarity.word: 85% ]

[ similarity.word: none  similarity.lemma: exact ]

[ similarity.lemma: none ]

the lexicon controls operate in a similar manner:

[  lexicon: KJV ]

That command is a concise way of setting two values. It is equivalent to this command

[ search.lexicon: KJV  render.lexicon: KJV ]

That is to say, lexicon is operative for searching and rendering. Like the similarity setting, the lexicon setting can also diverge between search and render parts. A common lexicon setting might be:

[ search.lexicon: both  render.lexicon: kjv ]

That setting would search both the KJV (aka AV) lexicon and a modernized lexicon (aka AVX), but verse rendering would only be in KJV.

#### 1.1.3 - Scoping Block

Sometimes we want to limit the scope of our search. Say that I want to find mentions of the serpent in Genesis. I can search only Genesis by executing this search:

serpent < Genesis

If I also want to search in Genesis and Revelation, this works:

serpent < Genesis < Revelation

Filters also allow Chapter and Verse specifications. To search for the serpent in Genesis Chapter 3, we can do this:

serpent < Genesis 3

Abbreviations are also supported:

vanity < sos < 1co



### 1.2 - Macro Directive

| Macro Directive *(follows the Selection Criteria)* | Syntax for applying tag to create a macro |
| -------------------------------------------------- | ----------------------------------------- |
| *apply*                                            | ***\|\| tag***                            |

**Table 1-2** - Syntax summary for the *apply* action in the Macro Directive of a Selection/Search imperative statement.

Tagged statements are also called macros. All macros are defined with a hash-tag (#); 

Macro tags cannot contain punctuation: only letters, numbers, hyphens, and underscores are permitted. However, macros are identified with a hash-tag (#).


Let’s say we want to name the search example from the previous section; We’ll call it *eternal-power*. To accomplish this, we can apply a tag to the statement below:

[ span: 7 similarity: 85% ] eternal power < Romans || eternal-power-romans

It’s that simple, now instead of typing the entire statement, we can utilize the macro by referencing our previously applied tag. Here is how the macro is utilized:

#eternal-power-romans



### 1.3 - Export Directive

| Export Directive  *(follows the Selection Criteria)* | Create file      | Create or Overwrite file | Create or Append File |
| ---------------------------------------------------- | ---------------- | :----------------------- | :-------------------- |
| *export*                                             | **>** *filename* | **=>** *filename*        | **>>** *filename*     |

**Table 1-3** - Syntax summary for the *export* action in the Export Directive of a Selection/Search imperative statement.

This would export all verses in Genesis 1 from the most previous search as html

[ format:html ] #in_beginning  > my-macro-output.html

This would export all verses for the executed macro as markdown

[ format:markdown ] #in_beginning  > my-macro-output.html

Combining only with a scoping black , we could append Genesis chapter two, to an existing file:

< Genesis 2  >> C:\users\my-user-name\documents\existing-file.md

Combining with a scoping black , we could replace the contents of an existing file with Genesis chapter three:

< Genesis 3  => C:\users\my-user-name\documents\existing-file.md



### 1.4 - Macro Utilization

The *use* action is supported in each of the three Selection Criteria blocks:

- Expression *(full macro utilization)*
- Settings *(partial macro utilization)*
- Scoping *(partial macro utilization)*

Each of the block types supports the *use* action. However, each block limited to, at most, one *use* action. The *use* action references either a tag for a macro or a statement id revealed by the @view imperative. As there are  a maximum of three blocks in the selection criteria, a statement could contain up to three *use* actions (one per block).

The expression block supports full macro utilization.  In the earlier example:

#eternal-power-romans

All settings, filters, and search criteria are utilized (this is called full macro utilization, and it can only occur in expression blocks)

Expression block macros sometimes undergo demotion. A macro within the expression block is demoted into a partial macro when a provided block within the selection criteria conflicts with the macro definition. Consider these examples:

Recall that the macro definition: [ span: 7 similarity: 85% ] eternal power < Romans || eternal-power-romans

| Macro Statement                       | Utilization level         | Explanation                                             |
| ------------------------------------- | ------------------------- | ------------------------------------------------------- |
| #eternal-power-romans                 | full macro utilization    | no conflicts                                            |
| #eternal-power-romans [ all:current ] | partial macro utilization | explicit settings replace any settings defined in macro |
| #eternal-power-romans < Acts          | partial macro utilization | explicit filter replaces any filters defined in macro   |
| #eternal-power-romans [span:7] < Acts | partial macro utilization | only the search expression is utilized from the macro   |

**Table 1-4** - Macro utilization in a Search Expression 

Outside of the expression block, partial macros *use* only the part of the macro that applies to the block type. For example, this clause utilizes only the settings defined within the macro.:

[ #eternal-power-romans ]

Likewise, in this example, this clause utilizes only the filters defined within the macro.

< #eternal-power-romans

Macro utilization within a block disallows all other entries within the block; macro utilization in a block is not compatible with any other entries in that same block.

Specifically, the following statements / clauses are not supported by ICL:

**NOT SUPPORTED:**  #eternal-power-romans without excuse  

**NOT SUPPORTED:**  [ #eternal-power-romans span:7 ]

**NOT SUPPORTED:**  < #eternal-power-romans < Acts

It should be noted that any macros referenced within a macro definition are expanded prior to applying the new tag. Therefore, subsequent redefinition of a previously referenced macro invocation never affects existing macro definitions. We call this macro-determinism.  All control settings are captured at the time that the tag is applied to the macro. This further assures that the same search results are returned each time the macro is referenced. Here is an example.

@set span = 2

in beginning || in_beginning

@set span = 3

#in_beginning [span:1] < genesis:1:1

***result:*** none

However, if the user desires the current settings to be used instead, a specialized control setting [ all:current ] represents all currently persisted settings; just include it in the statement (as show below). 

[ all:current ] #in_beginning < genesis:1:1

***result:*** Gen 1:1 In the beginning, God created ...

Similarly, another specialized setting is [ all:defaults ] ; that block represents default values for all settings. 

Still, a macro can be redefined/overwritten. This doesn't disable macro determinism, even though it feels like it does. The assumption is that the user is explicitly redefining the meaning of macro and ICL does not require an explicit @delete of the tag prior to re-applying. Here is an example:

[ #eternal-power-romans ] eternal power godhead without excuse < #eternal-power-romans || #eternal-power-romans

### 1.5 - History Utilization

Just like macro utilization, the *use* action is supported in each of the three Selection Criteria blocks:

- Expression *(full macro utilization)*
- Settings *(partial macro utilization)*
- Scoping *(partial macro utilization)*

Each of the block types supports the *use* action. However, each block limited to, at most, one *use* action. The *use* action references either a tag for a macro or a statement id revealed by  the @*view* imperative. As there are a maximum of three blocks in the selection criteria, a statement could contain up to three *use* actions (one per block).

Only the expression block supports full macro utilization.

Expression block macros sometimes undergo demotion. A historic utilization within the expression block is demoted into a partial macro when a provided block within the selection criteria conflicts with the macro definition. Assume that this command is identified by the @view command by id := 5:

[ span: 3 similarity: 85% ] "in ... beginning" < Genesis < John

| Statement          | Utilization level         | Explanation                                             |
| ------------------ | ------------------------- | ------------------------------------------------------- |
| #5                 | full macro utilization    | no conflicts                                            |
| #5 [ all:current ] | partial macro utilization | explicit settings replace any settings defined in macro |
| #5 < Acts          | partial macro utilization | explicit filter replaces any filters defined in macro   |
| #5 [span:7] < Acts | partial macro utilization | only the search expression is utilized from the macro   |

**Table 1-5** - History utilization in a Search Expression 

Outside of the expression block, partial *usage* applies by block type. For example, this clause utilizes only the settings defined where id = 5.

[ #5 ]

Likewise, in this example, this clause utilizes only the filters for id = 5.

< #5

Just like macros, utilization within a block disallows all other entries within the block; utilization in a block is not compatible with any other entries in that same block.

Specifically, the following statements / clauses are not supported by ICL:

**NOT SUPPORTED:**  #5 without excuse  

**NOT SUPPORTED:**  [ #5 span:7 ]

**NOT SUPPORTED:**  < #5 < Acts

It should be noted that any historic id references are expanded prior to applying the new tags for macros. As mentioned in the previous section, we call this macro-determinism.  Therefore, even if an id is removed from the command history with the @delete command, any macros that it was referenced within, continue to behave identically post-deletion.



## Section 2 - Configuration Statements

### 2.1 - Viewing Macros & Tags

| Action      | Syntax                                                       |
| ----------- | ------------------------------------------------------------ |
| **@delete** | *tag* <u>or</u> *wildcard* <u>or</u> -tags FROM <u>and/or</u> UNTIL<br/>**FROM parameter :** *from* yyyy/mm/dd<br/>**UNTIL parameter :** *until* yyyy/mm/dd |
| **@view**   | *tag* <u>or</u> *wildcard* <u>or</u> -tags <u>optional</u> FROM <u>and/or</u> UNTIL<br/>**FROM parameter :** *from* yyyy/mm/dd<br/>**UNTIL parameter :** *until* yyyy/mm/dd |
| **@absorb** | **permitted:** *tag*                                         |

**TABLE 2-1** -- **Tagging and viewing tagged statements**

##### Additional explicit macro commands:

Two additional explicit commands exist whereby a macro can be manipulated. We saw above how they can be defined and referenced. There are two additional ways commands that operate on macros: expansion and deletion.  In the last macro definition above where we created  #another-macro, the user could view an expansion by issuing this command:

@view another-macro

If the user wanted to remove this definition, the @delete action is used.  Here is an example:

@delete another-macro

If you want the same settings to be persisted to your current session that were in place during macro definition, the @absorb command will persist all settings for the macro into your current session

@absorb my-favorite-settings-macro 

**NOTE:**

​       @absorb also works with command history.

### 2.2 - Viewing History

| Verb        | Syntax Category | Parameters                                                   |
| ----------- | --------------- | ------------------------------------------------------------ |
| **@invoke** | Configuration   | ***id***                                                     |
| **@delete** | Configuration   | -history FROM <u>and/or</u> UNTIL<br/>**FROM parameter :** *from* *id* <u>or</u> *from* yyyy/mm/dd<br/>**UNTIL parameter :** *until* *id* <u>or</u> *until* yyyy/mm/dd |
| **@view**   | Configuration   | *id* <u>or</u> -history <u>optional</u> FROM <u>and/or</u> UNTIL<br/>**FROM parameter :** *from* *id* <u>or</u> *from* yyyy/mm/dd<br/>**UNTIL parameter :** *until* *id* <u>or</u> *until* yyyy/mm/dd |
| **@absorb** | Configuration   | ***id***                                                     |

**TABLE 2-2** -- **Viewing & Invoking statement history**

**COMMAND HISTORY** 

*@view* allows you to see your previous activity.  To show the last ten searches, type:

*@view* -history

To reveal all history up until now, type:

@view until now

To reveal all searches since January 1, 2024, type:

*@view* from 2024/1/1

To reveal for the single month of January 2024:

*@view* from 2024/1/1 until 2024/1/31

To reveal all history since id:5 [inclusive]:

*@view* from 5

All ranges are inclusive. 

**History Utilization**

The *use* command works for command-history works exactly the same way as it does for macros.  After issuing a *@view* command to show history, the user might receive a response as follows.

*@view*

1>  @set span = 7

2>  @set similarity=85

3> eternal power

And the use command can utilize any command listed.

#3

would be shorthand to for the search specified as:

eternal power

Again, *utilizing* a command from your command history is *used* just like a macro. Moreover, as with macros, control settings are persisted within your command history to provide invocation determinism. That means that control settings that were in place during the original command are utilized for the invocation.

Command history captures all settings. We have already discussed macro-determinism. Invoking commands by their ids behave exactly like macros. In other words, invoking command history never persists changes into your environment, unless you explicitly request such behavior with the @absorb command.

**RESETTING COMMAND HISTORY**

The @delete command can be used to remove <u>all</u> command history.

To remove all command history:

@delete -history -all

FROM / UNTIL parameters can limit the scope of the @delete command.

### 2.3 - Settings

| Action      | Parameters                                |
| ----------- | ----------------------------------------- |
| **@clear**  | *setting* or ALL                          |
| **@get**    | **optional:** *setting* or ALL or VERSION |
| **@set**    | *setting* **=** *value*                   |
| **@absorb** | ***id*** or ***tag***                     |

**TABLE 2-3.a** - **Listing of additional CONTROL actions**



**Export Format Options:**

| **Markdown**                            | **Text** (UTF8)                       | HTML             | JSON             | YAML             |
| --------------------------------------- | ------------------------------------- | ---------------- | ---------------- | ---------------- |
| @*format = md*<br/>@*format = markdown* | @*format = text*<br/>@*format = utf8* | @*format = html* | @*format = json* | @*format = yaml* |

**TABLE 2-3.b** - **@set** format command can be used to set the default content-formatting for for use with the export verb



| **example**              | **explanation**                                              | shorthand equivalent |
| ------------------------ | ------------------------------------------------------------ | -------------------- |
| **@set** search.span = 8 | Assign a control setting                                     | @span = 8            |
| **@get** search.span     | get a control setting                                        | @span                |
| **@clear** search.span   | Clear the setting; restores the setting to its default value | @clear span          |

**TABLE 2-3.c** - **set/clear/get** action operate on configuration settings

In all, ICL manifests five control names. Each allows all three actions: ***set***, ***clear***, and ***@get*** verbs. Table 9 lists all settings available in AV-Bible. AV-Bible supports two distinct orthographies [i.e. Contemporary Modern English (avx/modern), and/or Early Modern English (avx/kjv). These are selectable via ICL.

| Setting Name | Functional.Name  | Meaning                                                      | Values                                                       | Default Value |
| ------------ | ---------------- | ------------------------------------------------------------ | ------------------------------------------------------------ | ------------- |
| span         | search.span      | proximity distance limit                                     | 0 to 999 or verse                                            | 0 / verse     |
| lexicon      | -                | Streamlined syntax for setting search.lexicon and search. render to the same value | av or avx or dual<br/>(kjv or modern or both)                | n/a           |
| search       | search.lexicon   | the lexicon to be used for searching                         | av or avx or dual<br/>(kjv or modern or both)                | dual / both   |
| render       | render.lexicon   | the lexicon to be used for display/rendering                 | av/avx (kjv/modern)                                          | av / kjv      |
| format       | render.format    | format of results on output                                  | see Table 7                                                  | text / utf8   |
| similarity   |                  | Streamlined syntax for setting word & lemma to an identical value<br/>fuzzy phonetics matching threshold is between 1 and 99<br/>0 or *none* means: do not match on phonetics (use text only)<br/>100 or *exact* means that an *exact* phonetics match is expected | 33% to 99% [fuzzy] **or** ...<br>0 **or** *none*<br>100 **or** *exact* | 0 / none      |
| word         | similarity.word  | fuzzy phonetics matching as described above, but this prefix only affects similarity matching on the word. | 33% to 99% [fuzzy] **or** ...<br>0 **or** *none*<br>100 **or** *exact* | 0 / none      |
| lemma        | similarity.lemma | fuzzy phonetics matching as described above, but this prefix only affects similarity matching on the lemma. | 33% to 99% [fuzzy] **or** ...<br>0 **or** *none*<br>100 **or** *exact* | 0 / none      |
| revision     | grammar.revision | Not really a true setting: it works with the @get command to retrieve the revision number of the ICL grammar supported by AV-Engine. This value is read-only. | 4.x.yz                                                       | n/a           |
| ALL          |                  | ALL is an aggregate setting: it works with the @clear command to reset all variables above to their default values. It is used with @get to fetch all settings. It can also be used in the settings block of a statement to override values to default or the currently saved values for situations where a macro is utilized. | current<br/>**or**<br/>defaults                              | current       |

**TABLE 2-3.d** - Summary of AV-Bible Settings

The *@get* command fetches these values. The *@get* command requires a single argument. Examples are below:

*@get* span

@get format

All settings can be cleared using an explicit command:

@clear ALL

**Persistence of Settings**

It should be noted that there is a distinction between **@set** and and ***assign*** actions. The first action is an application configuration-imperative, and it is persistent (it affects all subsequent statements). Contrariwise, the ***assign*** action affects only the single statement wherein it is executed. We refer to this distinction as *persistence* vs *assignment*.

### 2.4 - Miscellaneous Information

**QUERYING DRIVER FOR VERSION INFORMATION**

This command reveals the current Grammar revision of the Imperative Control Language (ICL), implemented  by the command interpreter of AV-Bible:

@get revision

---

In general, the ICL command processor can be thought of as a stateless server. The only exceptions of its stateless nature are:

1. non-default settings assigned using the **@set** command

2. defined macro tags. 

3. command history

   

## Section 3 - Control Statements

### 3.1 - Program Help

To get general help, use this command:

*@help*

Or for specific topics:

*@help* find

*@help* set

@help export

etc ...

### 3.2 - Exiting the Application

Type this to terminate the app:

*@exit*

## Section 4 - Grammar and Design Elements

An object model to manifest the ICL grammar is depicted below:

![QCommand](./QCommand.png)

### 4.1 - Glossary of ICL Terminology

**Syntax Categories:** Each syntax category defines rules by which verbs can be expressed in the statement. 

**Actions:** Actions are complete verb-clauses issued in the imperative [you-understood].  Many actions have one or more parameters.  But just like English, a verb phrase can be a single word with no explicit subject and no explicit object.  Consider this English sentence:

Go!

The subject of this sentence is "you understood".  Similarly, all verbs are issued without an explicit subject. The object of the verb in the one word sentence above is also unstated.  ICL operates in an analogous manner.  Consider this English sentence:

Go Home!

Like the earlier example, the subject is "you understood".  The object this time is defined, and insists that "you" should go home.  Some verbs always have objects, others sometimes do, and still others never do. ICL follows this same pattern and some ICL verbs require direct-objects; and some do not.  In the various tables throughout this document, required and optional parameters are identified, These parameters represent the object of the verb within each respective table.

**Selection Criteria**: Selection what text to render is determined with a search expression, scoping filters, or both.

**Search Expression**: The Search Expression has fragments, and fragments have features. For an expression to match, all fragments must match (Logical AND). For a fragment to match, any feature must match (Logical OR). AND is represented by &. OR is represented by |.

**Unquoted SEARCH clauses:** an unquoted search clause contains one or more search fragments. If there is more than one fragment in the clause, then each fragment is logically AND’ed together.

**Quoted SEARCH clauses:** a quoted clause contains a single string of terms to search. An explicit match on the string is required. However, an ellipsis ( … ) can be used to indicate that other terms may silently intervene within the quoted string.

- It is called *quoted,* as the entire clause is sandwiched on both sides by double-quotes ( " )
- The absence of double-quotes means that the statement is unquoted

**Booleans and Negations:**

**and:** In Boolean logic, **and** means that all terms must be found. With ICL, **and** is represented by terms that appear within an unquoted clause. **And** logic is also available on each search-term by using the **&** operator.

**or:** In Boolean logic, **or** means that any term constitutes a match. With ICL, **and** is represented per each search-term by using the **|** operator.

**not:** In Boolean logic, means that the feature must not be found. With ICL, *not* is represented by the hyphen ( **-** ) and applies to individual features within a fragment of a search expression. It is best used in conjunction with other features, because any non-match will be included in results. 

hyphen ( **-** ) means that any non-match satisfies the search condition. Used by itself, it would likely return every verse. Therefore, it should be used judiciously.

### 4.2 - Specialized Search tokens in AV-Bible

The lexical search domain of ICL includes all words in the original KJV text. It can also optionally search using a modernized lexicon of the KJV (e.g. hast and has; this is controllable with the search.lexicon setting).  The table below lists linguistic extensions available in ICL.

| Search Term        | Operator Type                           | Meaning                                                      | Maps To                                                      | Mask   |
| ------------------ | --------------------------------------- | ------------------------------------------------------------ | ------------------------------------------------------------ | ------ |
| un\*               | wildcard (example)                      | starts with: un                                              | all lexicon entries that start with "un"                     | 0x3FFF |
| \*ness             | wildcard (example)                      | ends with: ness                                              | all lexicon entries that end with "ness"                     | 0x3FFF |
| un\*ness           | wildcard (example)                      | starts with: un<br/>ends with: ness                          | all lexicon entries that start with "un", and end with "ness" | 0x3FFF |
| \*profit\*         | wildcard (example)                      | contains: profit                                             | all lexicon entries that contain both "profit"               | 0x3FFF |
| \*pro\*fit\*       | wildcard (example)                      | contains: pro and fit                                        | all lexicon entries that contain both "pro" and "fit" (in any order) | 0x3FFF |
| un\*profit*ness    | wildcard (example)                      | starts with: un<br/>contains: profit<br/>ends with: ness     | all lexicon entries that start with "un", contain "profit", and end with "ness" | 0x3FFF |
| un\*pro\*fit\*ness | wildcard (example)                      | starts with: un<br/>contains: pro and fit<br/>ends with: ness | all lexicon entries that start with "un", contain "pro" and "fit", and end with "ness" | 0x3FFF |
| ~ʃɛpɝd*            | phonetic wildcard (example)             | Tilde marks the wildcard as phonetic (wildcards never perform sounds-alike searching) | All lexical entries that start with the sound ʃɛpɝd (this would include shepherd, shepherds, shepherding...) |        |
| ~ʃɛpɝdz            | sounds-alike search using IPA (example) | Tilde marks the search term as phonetic (and if similarity is set between 33 and 99, search handles approximate matching) | This would match the lexical entry "shepherds" (and possibly similar terms, depending on similarity threshold) |        |
| \\is\\             | lemma                                   | search on all words that share the same lemma as is: be, is, are, art, ... | be is are art ...                                            | 0x3FFF |
| /noun/             | lexical marker                          | any word where part of speech is a noun                      | POS12::0x010                                                 | 0x0FF0 |
| /n/                | lexical marker                          | synonym for /noun/                                           | POS12::0x010                                                 | 0x0FF0 |
| /verb/             | lexical marker                          | any word where part of speech is a verb                      | POS12::0x100                                                 | 0x0FF0 |
| /v/                | lexical marker                          | synonym for /verb/                                           | POS12::0x100                                                 | 0x0FF0 |
| /pronoun/          | lexical marker                          | any word where part of speech is a pronoun                   | POS12::0x020                                                 | 0x0FF0 |
| /pn/               | lexical marker                          | synonym for /pronoun/                                        | POS12::0x020                                                 | 0x0FF0 |
| /adjective/        | lexical marker                          | any word where part of speech is an adjective                | POS12::0xF00                                                 | 0x0FFF |
| /adj/              | lexical marker                          | synonym for /adjective/                                      | POS12::0xF00                                                 | 0x0FFF |
| /adverb/           | lexical marker                          | any word where part of speech is an adverb                   | POS12::0xA00                                                 | 0x0FFF |
| /adv/              | lexical marker                          | synonym for /adverb/                                         | POS12::0xA00                                                 | 0x0FFF |
| /determiner/       | lexical marker                          | any word where part of speech is a determiner                | POS12::0xD00                                                 | 0x0FF0 |
| /det/              | lexical marker                          | synonym for /determiner/                                     | POS12::0xD00                                                 | 0x0FF0 |
| /preposition/      | lexical marker                          | any word where part of speech is a preposition               | POS12::0x400                                                 | 0x0FF0 |
| /prep/             | lexical marker                          | any word where part of speech is a preposition               | POS12::0x400                                                 | 0x0FF0 |
| /1p/               | lexical marker                          | any word where it is inflected for 1st person (pronouns and verbs) | POS12::0x100                                                 | 0x3000 |
| /2p/               | lexical marker                          | any word where it is inflected for 2nd person (pronouns and verbs) | POS12::0x200                                                 | 0x3000 |
| /3p/               | lexical marker                          | any word where it is inflected for 3rd person (pronouns, verbs, and nouns) | POS12::0x300                                                 | 0x3000 |
| /singular/         | lexical marker                          | any word that is known to be singular (pronouns, verbs, and nouns) | POS12::0x400                                                 | 0xC000 |
| /plural/           | lexical marker                          | any word that is known to be plural (pronouns, verbs, and nouns) | POS12::0x800                                                 | 0xC000 |
| /WH/               | lexical marker                          | any word that is a WH word (e.g., Who, What, When, Where, How) | POS12::0xC00                                                 | 0xC000 |
| /BoB/              | transition marker                       | any word where it is the first word of the book (e.g. first word in Genesis) | TRAN::0xE0                                                   | 0xF0   |
| /BoC/              | transition marker                       | any word where it is the first word of the chapter           | TRAN::0x60                                                   | 0xF0   |
| /BoV/              | transition marker                       | any word where it is the first word of the verse             | TRAN::0x20                                                   | 0xF0   |
| /EoB/              | transition marker                       | any word where it is the last word of the book (e.g. last word in revelation) | TRAN::0xF0                                                   | 0xF0   |
| /EoC/              | transition marker                       | any word where it is the last word of the chapter            | TRAN::0x70                                                   | 0xF0   |
| /EoV/              | transition marker                       | any word where it is the last word of the verse              | TRAN::0x30                                                   | 0xF0   |
| /Hsm/              | segment marker                          | Hard Segment Marker (end) ... one of \. \? \!                | TRAN::0x40                                                   | 0x07   |
| /Csm/              | segment marker                          | Core Segment Marker (end) ... \:                             | TRAN::0x20                                                   | 0x07   |
| /Rsm/              | segment marker                          | Real Segment Marker (end) ... one of \. \? \! \:             | TRAN::0x60                                                   | 0x07   |
| /Ssm/              | segment marker                          | Soft Segment Marker (end) ... one of \, \; \( \) --          | TRAN::0x10                                                   | 0x07   |
| /sm/               | segment marker                          | Any Segment Marker (end)  ... any of the above               | TRAN::!=0x00                                                 | 0x07   |
| /_/                | punctuation                             | any word that is immediately marked for clausal punctuation  | PUNC::!=0x00                                                 | 0xE0   |
| /!/                | punctuation                             | any word that is immediately followed by an exclamation mark | PUNC::0x80                                                   | 0xE0   |
| /?/                | punctuation                             | any word that is immediately followed by a question mark     | PUNC::0xC0                                                   | 0xE0   |
| /./                | punctuation                             | any word that is immediately followed by a period (declarative) | PUNC::0xE0                                                   | 0xE0   |
| /-/                | punctuation                             | any word that is immediately followed by a hyphen/dash       | PUNC::0xA0                                                   | 0xE0   |
| /;/                | punctuation                             | any word that is immediately followed by a semicolon         | PUNC::0x20                                                   | 0xE0   |
| /,/                | punctuation                             | any word that is immediately followed by a comma             | PUNC::0x40                                                   | 0xE0   |
| /:/                | punctuation                             | any word that is immediately followed by a colon (information follows) | PUNC::0x60                                                   | 0xE0   |
| /'/                | punctuation                             | any word that is possessive, marked with an apostrophe       | PUNC::0x10                                                   | 0x10   |
| /)/                | parenthetical text                      | any word that is immediately followed by a close parenthesis | PUNC::0x0C                                                   | 0x0C   |
| /(/                | parenthetical text                      | any word contained within parenthesis                        | PUNC::0x04                                                   | 0x04   |
| /Italics/          | text decoration                         | italicized words marked with this bit in punctuation byte    | PUNC::0x02                                                   | 0x02   |
| /Jesus/            | text decoration                         | words of Jesus marked with this bit in punctuation byte      | PUNC::0x01                                                   | 0x01   |
| /delta/            | lexicon                                 | [archaic] word can be transformed into modern American English |                                                              |        |
| [type]             | named entity                            | Entities are recognized by MorphAdorner. They are also matched against Hitchcock's database. This functionality is experimental and considered BETA. | type=person man<br/>woman tribe city<br/>river mountain<br/>animal gemstone<br/>measurement any<br/>any_Hitchcock |        |
| \#FFFF             | PN+POS(12)                              | hexadecimal representation of bits for a PN+POS(12) value.   | See Digital-AV SDK                                           | uint16 |
| \#FFFFFFFF         | POS(32)                                 | hexadecimal representation of bits for a POS(32) value.      | See Digital-AV SDK                                           | uint32 |
| #string            | nupos-string                            | NUPOS string representing part-of-speech. This is the preferred syntax over POS(32), even though they are equivalent. NUPOS part-of-speech values have higher fidelity than the 16-bit PN+POS(12) representations. | See Part-of-Speech-for-Digital-AV.docx                       | uint32 |
| 99999:H            | Strongs Number                          | decimal Strongs number for the Hebrew word in the Old Testament | One of Strongs\[4\]                                          | 0x7FFF |
| 99999:G            | Strongs Number                          | decimal Strongs number for the Greek word in the New Testament | One of Strongs\[4\]                                          | 0x7FFF |

### 4-3 - Object Model to support ICL search tokens

An object model to support specialized ICL Search Tokens is depicted below:

![QFind](./QFind.png)

### 4.4 - ICL conformance to the Quelle specification

Quelle specifies two possible implementation levels:

- Level 1 [basic search support]
- Level 2 [search support includes also searching on part-of-speech tags]

ICL is a Level 2 Quelle implementation with augmented search capabilities. ICL extends Quelle to include AVX-Framework-specific constructs.  These extensions provide additional specialized search features and the ability to manage two distinct lexicons for the biblical texts.

1. ICL represents the biblical text with two substantially similar, but distinct, lexicons. The search.lexicon setting can be specified by the user to control which lexicon is to be searched. Likewise, the render.lexicon setting is used to control which lexicon is used for displaying the biblical text. As an example, the KJV text of "thou art" would be modernized to "you are".

   - AV/KJV *(a lexicon that faithfully represents the KJV bible; AV purists should select this setting)*

   - AVX/Modern *(a lexicon that that has been modernized to appear more like contemporary English)*

   - Dual/Both *(use both lexicons)*

   The Dual/Both setting for [search:] indicates that searching should consider both lexicons. The The Dual/Both setting for [render:] indicates that results should be displayed for both renderings [whether this is side-by-side or in-parallel depends on the format and the application where the display-rendering occurs]. Left unspecified, the lexicon setting applies to[search:] and [render:] components.

2. ICL provides support for fuzzy-match-logic. The similarity setting can be specified by the user to control the similarity threshold for approximate matching. An exact lexical match is expected when similarity is set to *exact* or 0.  Zero is not really a similarity threshold, but rather 0 is a synonym for *exact*.

   Approximate matches are considered when similarity is set between 33% and 99%. Similarity is calculated based upon the phonetic representation for the word.

   The minimum permitted similarity threshold is 33%. Any similarity threshold between 1% and 32% produces a syntax error.

   A similarity setting of *precise* or 100% is a special case that still uses phonetics, but expects a full phonetic match (e.g. "there" and "their" are a 100% phonetic match).

Av_bible uses the AV-1769 edition of the sacred text. It substantially agrees with the "Bearing Precious Seed" bibles, as published by local church ministries. The text itself has undergone review by Christian missionaries, pastors, and lay people since the mid-1990's. The original incarnation of the digitized AV-1769 text was implemented in the free PC/Windows app known as:

- AV-1995
- AV-1997
- AV-1999
- AV-2000
- AV-2007
- AV-2011
- AV-Bible for Windows

These releases were found at the [older/legacy] avbible.net website. Initially [decades ago], these releases were found on internet bulletin boards and the [now defunct] bible.advocate.com website.

Please see https://Digital-AV.org for additional information about the SDK.
