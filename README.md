# Quelle
Quelle is a specification. Its most generic form of the spec is known as Vanilla Quelle. Since Quelle could serve as a standard search syntax across various domains, tailoring for specific domains inevitably produces various dialects. It is recommended that each dialect conform to the "Vanilla Quelle" specification. A user-manual-of-sorts is provided here:

[Quelle/VanillaQuelle.md (github.com/kwonus)](https://github.com/kwonus/Quelle/blob/main/Vanilla-Quelle.md)

Initially, the manual served as the most complete specification. It was a specification-by-example (SBE). Now: a [PEG](https://bford.info/pub/lang/peg.pdf) grammar exists. This grammar not only provides a reference implementation using [Pest](https://pest.rs/) crate in Rust, but it also manifests a formal specification:

[Pin-Shot-Blue/src/quelle.pest (github.com/kwonus)](https://github.com/kwonus/Pin-Shot-Blue/blob/main/src/quelle.pest)

Quelle syntax agnostic about the search domain. Consequently, Quelle syntax has the potential for ubiquity.

In fact, a dialect of Quelle that is optimized for the bible search using AVX-Framework is the first complete reference implementation of Quelle. In the AVBible application, it is called Search-for-Truth (S4T) query language. It is found here:

[AVBible/Help/AV-Bible-S4T.html (github.com/kwonus)](https://github.com/kwonus/AVBible/blob/omega/Help/AV-Bible-S4T.html)

The formal grammar specification for S4T can be found at:

[Pinshot-Blue/src/avx-quelle.pest (github.com/kwonus)](https://github.com/kwonus/pinshot-blue/blob/main/src/avx-quelle.pest)





<br/></br>
# What's Available?
- A REST implementation is available in the project named [pinshot-svc](https://github.com/kwonus/pinshot-SVC) in my github repo
- A DLL (library) implementation is available in the project named [pinshot-blue](https://github.com/kwonus/pinshot-blue) in my github repo
