#!/usr/bin/perl
# Usage: perl activation.pl /service/info/activation
use Storable qw(lock_store);
my %keys = (
    'Drive' => ['AF921190','B91AD772'],
    'FCHBA' => [],
    'NIC10' => [],
    'NIC_1' => [],
    'NICFE' => [],
    );
lock_store(\%keys, $ARGV[0]);
