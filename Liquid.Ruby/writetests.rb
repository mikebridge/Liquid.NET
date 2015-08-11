#!/usr/bin/env ruby

require 'liquid'
#require 'active_support/core_ext/object/blank'
require 'active_support/all'

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
  File.open(file, 'r').read.split(/^[F.]*#.*\n/).drop(1).collect{ |x| x.strip }
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

def liquid_test_case(template, assigns, expected, empty=nil)
  {
      "input" => escape(template),
      "assigns" => escape(assigns),
      "expected" => escape(expected)
  }


end

def create_liquid_test_file_from_cases(inputfile, outputfile, classname, test_template)
  #puts inputfile
  #puts outputfile
  #puts test_template
  test_cases = read_tests_into_array(inputfile).each_slice(4).to_a.collect do |line|
     liquid_test_case(*line)
  end
  vars = {'classname' => classname,
          'sourcefile' => File.basename(inputfile),
          'tests' => test_cases,
          'exceptions' => []
  }
  #puts vars
  write_test_file(outputfile, vars, test_template)

  # .collect{ |x| test_line(x) }
  # #puts test_cases
  # vars = {'classname' => classname,
  #         'sourcefile' => File.basename(inputfile),
  #         'tests' => non_exception_results(test_cases),
  #         'exceptions' => exception_results(test_cases)
  # }
  # #puts(vars)
  #
  # write_test_file(outputfile, vars, test_template)

end

