def split_name(full_name):
    """
    Splits a full name string into first and last name components.
    Handles common formats including:
      - 'First Last', 'First Middle Last'
      - 'Last, First', 'Last-First'
      - Compound surnames (e.g. 'Berger-Brown', 'Von Berger')
      - Name suffixes (Jr., Sr., I-X) are stripped before parsing.

    Args:
        full_name (str): Raw full name string from OCR or data input.

    Returns:
        tuple: (first_name, last_name) as strings.
    """
    # Common name suffixes to reference
    suffix_list = [
        'I', 'I,', 'II', 'II, ','III', 'III,', 'IV', 'IV,', 'V', 'V,', 'VI', 'VI,', 'VII', 'VII,', 
        'VIII', 'VIII,', 'IX', 'IX,', 'X', 'X,', 'JR', 'JR.', 'JR,', 'SR', 'SR.', 'SR,'
    ]
    split_at_space = full_name.split(' ')
    name_split = [n for n in split_at_space if n.upper() not in suffix_list]
    contains_comma = False
    for name_segment in name_split:
        if ',' in name_segment:
            contains_comma = True
            break

    if not contains_comma:
        # last-first
        if len(name_split) == 1 and ('-' in name_split[0]):
            split_at_hyphen = name_split[0].split('-')
            first_name = split_at_hyphen[1]
            last_name = split_at_hyphen[0]
        # first last
        elif len(name_split) == 2 and (len(name_split[-1]) != 1):
            first_name = name_split[0]
            last_name = name_split[1]
        # last-first m & last-first m
        elif ('-' in name_split[0]) and (len(name_split[1]) == 1):
            split_at_hyphen = name_split[0].split('-')
            first_name = split_at_hyphen[1]
            last_name = split_at_hyphen[0]
        # first middle last
        elif len(name_split) == 3:
            first_name = name_split[0]
            last_name = name_split[2]
        # 'first m fathers_surname mothers_surname': surnames at end (add hyphen)
        elif (len(name_split) == 4) and (len(name_split[1]) == 1):
            first_name = name_split[0]
            last_name = name_split[2] + '-' + name_split[3]
        # 'Von last first m': surnames at beginning (add space between last names)
        elif (len(name_split) == 4) and (len(name_split[-1]) == 1):
            first_name = name_split[2]
            last_name = name_split[0] + ' ' + name_split[1]
        else:
            first_name = name_split[0]
            last_name = name_split[1]
    
    # has comma
    else:
        if len(name_split) > 2:
            for i in range(len(name_split)):
                if ',' in name_split[i]:
                    if i == 0:
                        last_name = name_split[i]
                        first_name = name_split[i+1]
                        break
                    else:
                        last_name = " ".join(name_split[:i+1])
                        first_name = name_split[i+1]
                        break
        else:
            first_name = name_split[1]
            last_name = name_split[0]

    # clean-up
    if not first_name[-1].isalpha():
        first_name = first_name[:-1]
    if not last_name[-1].isalpha():
        last_name = last_name[:-1]
    return first_name, last_name


# Full Names to test
full_name_list = [
    # space separators
    'Phil Berger', 'Phil D Berger', 'Phil D. Berger', 'Phil Dale Berger', 'Phil D Berger Zapatero',
    'Phil Dale Berger Jr.', 'Phil D Berger Jr', 'Phil Berger Jr', 'Von Berger Phil D Jr',
    'Von Berger Phil D',
    # comma & space separators
    'Berger, Phil', 'Berger, Phil, D', 'Berger, Phil, Dale', 'Berger, Phil Dale', 'Von Berger, Phil D Jr',
    # hyphen separators
    'Berger-Phil D', 'Berger-Phil D Jr', 'Berger-Phil', 'Phil D Berger-Brown',
    # additional characters
    'Phil D. Berger', 'Phil Berger;', 'Berger, Phil;', 'Phil Dale Berger;', 'Phil Berger.'
]

# Split full names into first and last names
for name in full_name_list:
    first_name, last_name = split_name(name)
    print(f'===== {name} =====')
    print(f'first name: {first_name}')
    print(f'last name: {last_name}\n')
