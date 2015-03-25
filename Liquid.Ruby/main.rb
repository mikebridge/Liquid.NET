#!/usr/bin/env ruby

require 'liquid'

def escape(str)
  return str.gsub('"','""')
end
  

def print_test(tmpl)
  puts escape(tmpl);
  puts Liquid::Template.parse(tmpl).render
end

print_test "foo {{ \"bar\" | upcase }}"
