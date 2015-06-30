# Liquid.NET

http://mikebridge.github.io/Liquid.NET/


## TODO

- tablerow tag
- ifchanged tag
- allow keyword tag names as variables (e.g. "limit:limit")
- allow expression in case (not just variable)
- to_number filter

- implement `{%- -%}` to trim spaces
- several test fixes

## ALSO TODO

- I think the option syntax should be used in the filters
- The non-custom filters should be directly called by the RenderingVisitor, rather than via dynamic instantiation.
