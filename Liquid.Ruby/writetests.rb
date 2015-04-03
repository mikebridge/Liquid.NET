#!/usr/bin/env ruby

require 'liquid'

def escape(str)
  str.gsub('"','""')
end

def eval_tmpl(tmpl, vars = {})
  begin
    return Liquid::Template.parse(tmpl).render(vars);
  rescue Liquid::SyntaxError => err
    return "EXCEPTION: " + err.message
  end
end

def read_tests_into_array(file)
  File.open(file, 'r').read.split(/^#.*\n/).collect{ |x| x.strip }
end

def write_test_file(output_filename, vars, test_template)

  File.open(output_filename, 'w') {
      |file| file.write(eval_tmpl(test_template, vars))
  }

end

def test_line(tmpl)

  { "input" => escape(tmpl),
    "expected" =>  eval_tmpl(tmpl)
  }
end

def exception_results(test_cases)
  test_cases.select { |x| x["expected"].index("EXCEPTION: ") == 0 }
end

def non_exception_results(test_cases)
  test_cases.select { |x| x["expected"].index("EXCEPTION: ") != 0 }
end


def create_test_file_from_cases(inputfile, outputfile, classname, test_template)
  test_cases = read_tests_into_array(inputfile).collect{ |x| test_line(x) }
  #puts test_cases
  vars = {'classname' => classname,
          'sourcefile' => File.basename(inputfile),
          'tests' => non_exception_results(test_cases),
          'exceptions' => exception_results(test_cases)
  }
  #puts(vars)

  write_test_file(outputfile, vars, test_template)

end

# FILTER TESTS

# filters = {
#     'classname' => 'FilterTests',
#     'tests' => [
#     test_line("{{ \"3\" | times: \"3\" }}")
# ]}
#
# filter_output = File.join(homedir, 'Liquid.NET.Tests/Ruby/FilterTests.cs')
# File.open(filter_output, 'w') {
#    |file| file.write(eval_tmpl(test_template, filters))
# }

# ERROR TESTS

# filters = {
#     'classname' => 'ErrorTests',
#     'tests' => [
#         test_line("{{ \"1\" | divided_by: \"0\" }}"),
#         test_line("{{ \"x\" | divided_by: \"1\" }}"),
#         test_line('{{ 1 | unk_filter }}'),
#         test_line("{{ \"test,test\" | split: }}"),
#         test_line('{% unknown_tag %}')
# ]}

#error_output = File.join(homedir, 'Liquid.NET.Tests/Ruby/ErrorTests.cs')
#error_input = File.join(testdir, "errors.txt")
#File.open(error_output, 'w') {
#    |file| file.write(eval_tmpl(test_template, read_tests_into_array(error_input)))
#}


#puts filters
#puts eval_tmpl(test_template, filters)

#
# filters = { 'errortests' => [
#     { "input" => escape("{{ \"\" | times: \"3\" }}"), "expected" => escape('9') }
# ]}
#
# test_template = File.read('tests.liquid')
# filter_output = File.join(home, 'Liquid.NET.Tests/Ruby/ErrorTests.cs')
# File.open(filter_output, 'w') {
#     |file| file.write(write_test_file(test_template, filters))
# }
# #puts write_test_file(test_template, filters)

