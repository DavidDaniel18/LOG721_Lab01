var MyContract = artifacts.require('StudentContract')

module.exports = function (deployer) {
  deployer.deploy(MyContract)
}
