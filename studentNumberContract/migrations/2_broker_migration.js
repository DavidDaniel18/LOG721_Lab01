var MyContract = artifacts.require('MessageBroker')

module.exports = function (deployer) {
  deployer.deploy(MyContract)
}
